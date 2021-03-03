namespace TestSyncConsole.TestAssemblyManagement
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Serilog;

    public class TestAssembly : ITestAssembly
    {
        private readonly string assemblyPath;
        private readonly TestStrategy testStrategy;

        public TestAssembly(ILaunchSettings launchSettings)
        {
            this.assemblyPath = launchSettings.Arguments.AssemblyPath;
            this.testStrategy = launchSettings.Arguments.TestStratgey;
        }

        public IEnumerable<UITest> GetTestMethods()
        {
            Assembly assembly;

            try
            {
                assembly = Assembly.LoadFrom(this.assemblyPath);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "There was as issue loading the assembly");
                throw e;
            }

            ITestStrategy testStrategy = this.testStrategy switch
            {
                TestStrategy.NUnit => new NUnitStrategy(),
                _ => new SpecflowPlusStrategy()
            };

            return testStrategy.GetTests(assembly);
        }
    }
}