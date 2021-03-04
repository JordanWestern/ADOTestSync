namespace TestSyncConsole.WorkItems
{
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using TestSyncConsole.WorkItemManagement;

    public interface IWorkItemTracker
    {
        Task<WorkItemBatchGetResponse> PostWorkItemBatchQueryAsync(WorkItemBatchGetRequest workItemBatchGetRequest);

        Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage);
    }
}