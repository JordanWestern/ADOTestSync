namespace TestSyncConsole.TestPlans
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public interface ITestPlansController
    {
        Task<IEnumerable<WorkItem>> GetAutomatedTestCasesAsync();
        Task UploadTestCasesAsync(IEnumerable<UITest> uITests);
    }
}