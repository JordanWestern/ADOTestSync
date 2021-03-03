namespace TestSyncConsole.TestAssemblyManagement
{
    using System.Reflection;

    public interface ITestStrategy
    {
        UITest[] GetTests(Assembly testAssembly);
    }
}