namespace TestSyncConsole.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using TestSyncConsole.WorkItemManagement;
    using TestSyncConsole.WorkItemTracking;

    public class TestPlansController : ITestPlansController
    {
        private readonly IWorkItemTracker workItemTracker;

        public TestPlansController(IWorkItemTracker workItemTracker)
        {
            this.workItemTracker = workItemTracker;
        }

        public async Task<List<WorkItem>> GetAutomatedTestCasesAsync()
        {
            var workItemBatchQueries = new List<Task<WorkItemBatchGetResponse>>();

            var workItemQueryResult = await this.workItemTracker.PostWorkItemQueryAsync(WorkItemQuery.GetAutomatedTestCases);

            var workItemCount = workItemQueryResult.WorkItems.Count();

            for (int i = 0; i < workItemCount; i += 200)
            {
                var request = new WorkItemBatchGetRequest
                {
                    Fields = new string[] { "System.Title" },
                    Ids = workItemQueryResult.WorkItems.Skip(i).Take(200).Select(workItem => workItem.Id)
                };

                workItemBatchQueries.Add(this.workItemTracker.PostWorkItemBatchQueryAsync(request));
            }

            await Task.WhenAll(workItemBatchQueries);

            var workItems = new List<WorkItem>();

            foreach (var workItemBatchQuery in workItemBatchQueries)
            {
                workItems.AddRange(workItemBatchQuery.Result.WorkItems);
            }

            return workItems;
        }
    }
}