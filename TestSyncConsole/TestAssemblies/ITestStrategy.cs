﻿namespace TestSyncConsole.TestAssemblies
{
    using System.Collections.Generic;
    using System.Reflection;

    public interface ITestStrategy
    {
        IEnumerable<UITest> GetTests(Assembly testAssembly);
    }
}