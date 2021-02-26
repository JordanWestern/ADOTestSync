using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestSyncConsole.Services;
using TestSyncConsole.WorkItem;

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
                })
                .UseSerilog()
                .Build();

            Log.Logger.Information("Starting service...");

            var workItemTracker = ActivatorUtilities.CreateInstance<WorkItemTracker>(host.Services);

            Log.Logger.Information("Posting work item query: {0}", nameof(WorkItemQuery.GetAutomatedTestCases));

            var result = await workItemTracker.PostWorkItemQueryAsync(WorkItemQuery.GetAutomatedTestCases);

            Log.Logger.Information("Sucessfully retrieved {0} test cases", result.WorkItems.Count());
        }

        static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}