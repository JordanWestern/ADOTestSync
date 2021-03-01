using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestSyncConsole.Controllers
{
    public interface ITestPlansController
    {
        Task<List<WorkItem>> GetAutomatedTestCasesAsync();
    }
}