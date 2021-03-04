namespace TestSyncConsole.WorkItems
{
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
    using TestSyncConsole.WorkItemManagement;

    public interface IWorkItemTracker
    {
        Task CreateWorkItemAsync(JsonPatchDocument jsonPatchOperations, string workItemType);

        Task<WorkItemBatchGetResponse> PostWorkItemBatchQueryAsync(WorkItemBatchGetRequest workItemBatchGetRequest);

        Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage);
    }
}