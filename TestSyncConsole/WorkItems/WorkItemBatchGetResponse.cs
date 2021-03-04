namespace TestSyncConsole.WorkItemManagement
{
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Newtonsoft.Json;

    public class WorkItemBatchGetResponse
    {
        [JsonProperty("value")]
        public WorkItem[] WorkItems { get; set; }
    }
}