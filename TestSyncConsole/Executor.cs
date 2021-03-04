namespace TestSyncConsole
{
    using System.Linq;
    using System.Threading.Tasks;
    using Serilog;
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
            Log.Information("Execution starting");

            var testsFromAssembly = this.testAssembly.GetTestMethods();
            Log.Information("Found {0} tests in {1}", testsFromAssembly.Count(), this.launchSettings.Arguments.AssemblyPath);

            var testsFromAzure = (await this.testPlansController.GetAutomatedTestCasesAsync()).ToUITests();
            Log.Information("Found {0} tests in {1}", testsFromAzure.Count(), $"{this.launchSettings.Arguments.Organisation}/{this.launchSettings.Arguments.Project}");

            var (notInAssembly, notInAzure) = TestComparer.Compare(testsFromAssembly, testsFromAzure);

            var notInAssemblyCount = notInAssembly.Count();
            var notInAzureCount = notInAzure.Count();

            if (notInAssemblyCount == 0 && notInAzureCount == 0)
            {
                Log.Information("Tests are up to date, no action required");
                return;
            }

            if (notInAzureCount > 0)
            {
                Log.Information("{0} tests from your assembly are missing from Azure and will be uploaded", notInAzureCount);
                await this.testPlansController.UploadTestCasesAsync(notInAzure);
            }

            if (notInAssemblyCount > 0)
            {
                Log.Information("{0} tests from Azure no longer exist in your test assembly and will be deleted", notInAssemblyCount);

                // Delete the tests from Azure
            }
        }
    }
}
