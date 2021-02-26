using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Threading.Tasks;

namespace TestSyncConsole.Services
{
    public interface IAzureService
    {
        Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage);
    }
}