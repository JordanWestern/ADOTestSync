namespace TestSyncConsole
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Serilog;
    using TestSyncConsole.Constants;
    using TestSyncConsole.Extensions;
    using TestSyncConsole.TestAssemblies;
    using TestSyncConsole.TestPlans;
    using TestSyncConsole.Utilities;

    public class Executor : IExecutor
    {
        private readonly ITestPlansController testPlansController;
        private readonly ITestAssembly testAssembly;
        private readonly ILaunchSettings launchSettings;

        public Executor(ITestPlansController testPlansController, ITestAssembly testAssembly, ILaunchSettings launchSettings)
        {
            this.testPlansController = testPlansController;
            this.testAssembly = testAssembly;
            this.launchSettings = launchSettings;
        }

        public async Task ExecuteAsync()
        {
            var testsFromAssembly = this.testAssembly.GetTestMethods();
            Log.Information("Found {0} tests in {1}", testsFromAssembly.Count(), this.launchSettings.Arguments.AssemblyPath);

            var testsFromAzure = await this.testPlansController.GetAutomatedTestCasesAsync();
            Log.Information("Found {0} tests in {1}", testsFromAzure.Count(), $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}");

            var (notInAssembly, notInAzure) = TestComparer.Compare(testsFromAssembly, testsFromAzure.ToUITests());

            var notInAssemblyCount = notInAssembly.Count();
            var notInAzureCount = notInAzure.Count();

            if (notInAssemblyCount == 0 && notInAzureCount == 0)
            {
                Log.Information("Your test project and Azure are in sync so no action is required");
                return;
            }

            if (notInAzureCount > 0)
            {
                Log.Information("{0} test(s) from your assembly are missing from Azure and will be uploaded", notInAzureCount);
                await this.testPlansController.UploadTestCasesAsync(notInAzure);
            }
            else
            {
                Log.Information("Azure is up to date with your test project");
            }

            if (notInAssemblyCount > 0)
            {
                Log.Information("{0} test(s) from Azure no longer exist in your test assembly and will be deleted from Azure", notInAssemblyCount);
                await this.testPlansController.DeleteTestCasesAsync(testsFromAzure.Where(x => notInAssembly.Any(y => y.Guid.Equals(Guid.Parse(x.Fields[WorkItemFields.AutomatedTestId].ToString())))));
            }
            else
            {
                Log.Information("Azure does not contain anything that does not exist in your test assembly");
            }
        }
    }
}
