namespace TestSyncConsole.Extensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.Common;
    using TestSyncConsole.Constants;

    public static class WorkItemExtensions
    {
        public static IEnumerable<UITest> ToUITests(this IEnumerable<WorkItem> workItems)
        {
            var tests = new List<UITest>();

            workItems.ForEach(workItem => tests.Add(new UITest
            {
                ScenarioName = workItem.Fields[WorkItemFields.Title].ToString(),
                Module = workItem.Fields[WorkItemFields.AutomatedTestStorage].ToString(),
                FullyQualifiedName = workItem.Fields[WorkItemFields.AutomatedTestName].ToString(),
                Guid = Guid.Parse(workItem.Fields[WorkItemFields.AutomatedTestId].ToString()),
                Tags = workItem.Fields[WorkItemFields.Tags].ToString().Split(';')
            }));

            return tests;
        }
    }
}
