using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using System.IO;

namespace TestSyncConsole
{
    class Program
    {
        static void Main(string[] args)
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
                    services.AddSingleton<IAzureConfiguration, AzureConfiguration>();
                    services.AddTransient<IAzureService, AzureService>();
                })
                .UseSerilog()
                .Build();

            var azureService = ActivatorUtilities.CreateInstance<AzureService>(host.Services);

            azureService.UploadTests();
        }

        static void BuildConfiguration(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}
