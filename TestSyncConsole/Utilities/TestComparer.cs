namespace TestSyncConsole.Utilities
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TestComparer
    {
        // TODO: needs to be more simple but works... Could not get Except() or Intersect() to work as expected.
        public static (IEnumerable<UITest> testsNotInAssembly, IEnumerable<UITest> testsNotInAzure) Compare(IEnumerable<UITest> testsFromAssembly, IEnumerable<UITest> testsFromAzure)
        {
            return (testsFromAzure.Where(x => !testsFromAssembly.Any(y => y.Guid == x.Guid)), testsFromAssembly.Where(x => !testsFromAzure.Any(y => y.Guid == x.Guid)));
        }
    }
}
