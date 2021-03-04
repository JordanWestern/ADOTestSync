namespace TestSyncConsole.TestAssemblies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class NUnitStrategy : ITestStrategy
    {
        public IEnumerable<UITest> GetTests(Assembly testAssembly)
        {
            throw new NotImplementedException();
        }
    }
}
