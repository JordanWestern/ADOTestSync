namespace TestSyncConsole.TestAssemblyManagement
{
    using System.Collections.Generic;

    public interface ITestAssembly
    {
        IEnumerable<UITest> GetTestMethods();
    }
}