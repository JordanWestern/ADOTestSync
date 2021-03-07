namespace TestSyncConsole
{
    using System;

    public class UITest
    {
        public string ScenarioName { get; set; }

        public string FullyQualifiedName { get; set; }

        public string[] Tags { get; set; }

        public string Module { get; set; }

        public Guid Guid { get; set; }

        public bool ExceedsCharLimit => this.FullyQualifiedName.Length > 256;
    }
}