namespace TestSyncConsole
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using TestSyncConsole.Controllers;
    using TestSyncConsole.Services;
    using TestSyncConsole.TestAssemblyManagement;
    using TestSyncConsole.WorkItemTracking;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();

            BuildConfiguration(configurationBuilder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationBuilder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ILaunchSettings>(new LaunchSettings(args));
                    services.AddTransient<IAzureService, AzureService>();
                    services.AddTransient<IWorkItemTracker, WorkItemTracker>();
                    services.AddTransient<ITestPlansController, TestPlansController>();
                    services.AddTransient<ITestAssembly, TestAssembly>();
                })
                .UseSerilog()
                .Build();

            Log.Logger.Information("Starting service...");

            var testAssembly = ActivatorUtilities.CreateInstance<TestAssembly>(host.Services);

            testAssembly.GetTestMethods();
        }

        private static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(GetBasePath())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        // TODO: Look into getting the working directory for the published package as Directory.GetCurrentDirectory() does not do the trick!
        private static string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}