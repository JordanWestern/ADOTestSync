using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TestSyncConsole.Services;

namespace TestSyncConsole.WorkItem
{
    public class WorkItemTracker : IWorkItemTracker
    {
        private readonly IAzureService azureService;
        private readonly ILaunchSettings launchSettings;

        public WorkItemTracker(IAzureService azureService, ILaunchSettings launchSettings)
        {
            this.azureService = azureService;
            this.launchSettings = launchSettings;
        }

        public async Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage)
        {
            return JsonConvert.DeserializeObject<WorkItemQueryResult>(
                await this.azureService.PostAsync(
                    $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}/_apis/wit/wiql?api-version=6.0", workItemQueryLanguage).Result.Content.ReadAsStringAsync());
        }
    }
}
