﻿using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace TestSyncConsole.WorkItemTracking
{
    static class WorkItemQuery
    {
        public static Wiql GetAutomatedTestCases => new Wiql { Query = "SELECT * FROM WorkItems WHERE [Work Item Type] = 'Test Case' AND [Automation status] = 'Automated'" };
    }
}