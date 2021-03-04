namespace TestSyncConsole.TestAssemblies
{
    using System.Collections.Generic;

    public interface ITestAssembly
    {
        IEnumerable<UITest> GetTestMethods();
    }
}