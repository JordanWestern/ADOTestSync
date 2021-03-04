namespace TestSyncConsole.Utilities
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestComparer
    {
        public static (IEnumerable<UITest> testsNotInAssembly, IEnumerable<UITest> testsNotInAzure) Compare(IEnumerable<UITest> testsFromAssembly, IEnumerable<UITest> testsFromAzure)
        {
            return (testsFromAzure.Except(testsFromAssembly), testsFromAssembly.Except(testsFromAzure));
        }
    }
}
