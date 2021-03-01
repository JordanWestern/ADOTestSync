using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TestSyncConsole.Controllers;
using TestSyncConsole.Services;
using TestSyncConsole.WorkItemTracking;

namespace TestSyncConsole
{
    class Program
    {
        static async Task Main(string[] args)
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
                })
                .UseSerilog()
                .Build();

            Log.Logger.Information("Starting service...");

            var testPlansController = ActivatorUtilities.CreateInstance<TestPlansController>(host.Services);

            Log.Information("Fetching test cases...");

            var testcases = await testPlansController.GetAutomatedTestCasesAsync();

            Log.Information("Retrieved {0} test cases from Azure:", testcases.Count);

            foreach (var testCase in testcases)
            {
                Log.Information("ID: {0} - Title: {1}", testCase.Id, testCase.Fields["System.Title"]);
            }
        }

        static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(GetBasePath())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        //TODO: Look into getting the working directory for the published package as Directory.GetCurrentDirectory() does not do the trick!
        static string GetBasePath() 
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
    }
}