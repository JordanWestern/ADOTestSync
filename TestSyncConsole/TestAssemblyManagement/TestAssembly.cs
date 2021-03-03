namespace TestSyncConsole.TestAssemblies
{
    using System;
    using System.Reflection;
    using Serilog;
    using TestSyncConsole.TestAssemblyManagement;

    public class TestAssembly
    {
        private readonly string assemblyPath;
        private readonly TestStrategy testStrategy;

        public TestAssembly(ILaunchSettings launchSettings)
        {
            this.assemblyPath = launchSettings.Arguments.AssemblyPath;
            this.testStrategy = launchSettings.Arguments.TestStratgey;
        }

        public UITest[] GetTestMethods()
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFile(this.assemblyPath);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "There was as issue loading the assembly");
                throw e;
            }

            ITestStrategy testStrategy = this.testStrategy switch
            {
                TestStrategy.NUnit => new NUnitStrategy(),
                _ => new SpecFlowPlusStrategy(),
            };
            return testStrategy.GetTests(assembly);
        }
    }
}