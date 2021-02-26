using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Threading.Tasks;

namespace TestSyncConsole.WorkItem
{
    public interface IWorkItemTracker
    {
        Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage);
    }
}