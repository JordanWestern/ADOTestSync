namespace TestSyncConsole.TestPlans
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
    using Serilog;
    using TestSyncConsole.Constants;
    using TestSyncConsole.WorkItemManagement;
    using TestSyncConsole.WorkItems;

    public class TestPlansController : ITestPlansController
    {
        private readonly IWorkItemTracker workItemTracker;

        public TestPlansController(IWorkItemTracker workItemTracker)
        {
            this.workItemTracker = workItemTracker;
        }

        public async Task<IEnumerable<WorkItem>> GetAutomatedTestCasesAsync()
        {
            var workItemBatchQueries = new List<Task<WorkItemBatchGetResponse>>();
            var workItemQueryResult = await this.workItemTracker.PostWorkItemQueryAsync(WorkItemQuery.GetAutomatedTestCases);
            var workItemCount = workItemQueryResult.WorkItems.Count();

            for (int i = 0; i < workItemCount; i += 200)
            {
                var request = new WorkItemBatchGetRequest
                {
                    Fields = new string[]
                    {
                        WorkItemFields.Title,
                        WorkItemFields.AutomatedTestStorage,
                        WorkItemFields.AutomatedTestName,
                        WorkItemFields.Tags,
                        WorkItemFields.AutomatedTestId
                    },

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

        public async Task UploadTestCasesAsync(IEnumerable<UITest> uITests)
        {
            foreach (var uITest in uITests)
            {
                if (uITest.ExceedsCharLimit)
                {
                    Log.Error("The fully qualified name for {0} exceeds the character limit and will be excluded from the upload", uITest.ScenarioName);
                    return;
                }

                var jsonPatchOperations = new JsonPatchDocument
                {
                    new JsonPatchOperation
                    {
                        Value = uITest.ScenarioName,
                        From = null,
                        Operation = Operation.Add,
                        Path = $"/fields/{WorkItemFields.Title}"
                    },
                    new JsonPatchOperation
                    {
                        Value = uITest.Module,
                        From = null,
                        Operation = Operation.Add,
                        Path = $"/fields/{WorkItemFields.AutomatedTestStorage}"
                    },
                    new JsonPatchOperation
                    {
                        Value = uITest.FullyQualifiedName,
                        From = null,
                        Operation = Operation.Add,
                        Path = $"/fields/{WorkItemFields.AutomatedTestName}"
                    },
                    new JsonPatchOperation
                    {
                        Value = string.Join(";", uITest.Tags),
                        From = null,
                        Operation = Operation.Add,
                        Path = $"/fields/{WorkItemFields.Tags}"
                    },
                    new JsonPatchOperation
                    {
                        Value = uITest.Guid,
                        From = null,
                        Operation = Operation.Add,
                        Path = $"/fields/{WorkItemFields.AutomatedTestId}"
                    }
                };

                try
                {
                    await this.workItemTracker.CreateWorkItemAsync(jsonPatchOperations, WorkItemTypes.TestCase);
                    Log.Information("Uploaded {0} successfully", uITest.ScenarioName);
                }
                catch (Exception e)
                {
                    Log.Error(e, "There was an issue uploading {0}", uITest.ScenarioName);
                }
            }
        }

        public async Task DeleteTestCasesAsync(IEnumerable<WorkItem> workItems)
        {
            foreach (var workItem in workItems)
            {
                try
                {
                    await this.workItemTracker.DeleteTestCaseWorkItemAsync(workItem.Id);
                    Log.Information("Deleted test case {0}: {1} successfully", workItem.Id, workItem.Fields[WorkItemFields.Title].ToString());
                }
                catch (Exception e)
                {
                    Log.Error(e, "There was an issue deleting work item {0}", workItem.Id);
                }
            }
        }
    }
}