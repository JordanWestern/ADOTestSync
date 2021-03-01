using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;

namespace TestSyncConsole.WorkItemManagement
{
    public class WorkItemBatchGetResponse
    {
        [JsonProperty("value")]
        public WorkItem[] WorkItems { get; set; }
    }
}