namespace TestSyncConsole.WorkItems
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
    using Newtonsoft.Json;
    using TestSyncConsole.Services;
    using TestSyncConsole.WorkItemManagement;

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
                await this.azureService.PostAsJsonAsync(
                    $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}/_apis/wit/wiql?api-version=6.0", workItemQueryLanguage).Result.Content.ReadAsStringAsync());
        }

        public async Task<WorkItemBatchGetResponse> PostWorkItemBatchQueryAsync(WorkItemBatchGetRequest workItemBatchGetRequest)
        {
            return JsonConvert.DeserializeObject<WorkItemBatchGetResponse>(
                await this.azureService.PostAsJsonAsync(
                    $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}/_apis/wit/workitemsbatch?api-version=6.0", workItemBatchGetRequest).Result.Content.ReadAsStringAsync());
        }

        public async Task CreateWorkItemAsync(JsonPatchDocument jsonPatchOperations, string workItemType)
        {
            try
            {
                await this.azureService.PostAsync(
                        $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}/_apis/wit/workitems/${workItemType}?api-version=6.0", new StringContent(JsonConvert.SerializeObject(jsonPatchOperations), Encoding.UTF8, "application/json-patch+json"));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}