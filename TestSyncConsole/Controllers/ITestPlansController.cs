namespace TestSyncConsole.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public interface ITestPlansController
    {
        Task<List<WorkItem>> GetAutomatedTestCasesAsync();
    }
}