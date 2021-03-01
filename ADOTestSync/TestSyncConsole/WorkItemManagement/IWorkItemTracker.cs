using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Threading.Tasks;
using TestSyncConsole.WorkItemManagement;

namespace TestSyncConsole.WorkItemTracking
{
    public interface IWorkItemTracker
    {
        Task<WorkItemBatchGetResponse> PostWorkItemBatchQueryAsync(WorkItemBatchGetRequest workItemBatchGetRequest);
        Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage);
    }
}