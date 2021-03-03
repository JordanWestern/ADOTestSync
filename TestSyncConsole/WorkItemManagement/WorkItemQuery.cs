namespace TestSyncConsole.WorkItemTracking
{
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public static class WorkItemQuery
    {
        public static Wiql GetAutomatedTestCases => new Wiql { Query = "SELECT * FROM WorkItems WHERE [Work Item Type] = 'Test Case' AND [Automation status] = 'Automated'" };
    }
}