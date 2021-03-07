namespace TestSyncConsole
{
    using System;
    using System.Collections.Generic;

    public class UITest : IEqualityComparer<UITest>
    {
        public string ScenarioName { get; set; }

        public string FullyQualifiedName { get; set; }

        public string[] Tags { get; set; }

        public string Module { get; set; }

        public Guid Guid { get; set; }

        public bool ExceedsCharLimit => this.FullyQualifiedName.Length > 256;

        public bool Equals(UITest x, UITest y)
        {
            return x != null && y != null && (ReferenceEquals(x, y) || (x.Guid == y.Guid));
        }

        public int GetHashCode(UITest uITest)
        {
            return this.Guid.GetHashCode();
        }
    }
}