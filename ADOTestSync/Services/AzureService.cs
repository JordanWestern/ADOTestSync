using Microsoft.Extensions.Logging;

namespace Services
{
    public class AzureService : IAzureService
    {
        private readonly ILogger<AzureService> logger;
        private readonly IAzureConfiguration azureConfiguration;

        public AzureService(ILogger<AzureService> logger, IAzureConfiguration azureConfiguration)
        {
            this.logger = logger;
            this.azureConfiguration = azureConfiguration;
        }

        public void UploadTests()
        {
            logger.LogInformation(
                "Domain {domain}, Organisation {organisation}, Project {project}, PersonalAccessToken {personalAccessToken}",
                azureConfiguration.Domain,
                azureConfiguration.Organisation,
                azureConfiguration.Project,
                azureConfiguration.PersonalAccessToken);
        }
    }
}
