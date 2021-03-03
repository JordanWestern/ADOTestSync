namespace TestSyncConsole.TestAssemblyManagement
{
    using System;
    using System.Reflection;

    public class NUnitStrategy : ITestStrategy
    {
        public UITest[] GetTests(Assembly testAssembly)
        {
            throw new NotImplementedException();
        }
    }
}
