using Microsoft.Extensions.Configuration;

namespace Services
{
    public class AzureConfiguration : IAzureConfiguration
    {
        private readonly IConfiguration configuration;

        public AzureConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Domain => configuration.GetValue<string>("AzureConfiguration:Domain");

        public string Organisation => configuration.GetValue<string>("AzureConfiguration:Organisation");

        public string Project => configuration.GetValue<string>("AzureConfiguration:Project");

        public string PersonalAccessToken => configuration.GetValue<string>("AzureConfiguration:PersonalAccessToken");
    }
}
