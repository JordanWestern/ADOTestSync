namespace TestSyncConsole
{
    using CommandLine;

    public class Arguments : IArguments
    {
        [Option(longName: "organisation", Required = true, HelpText = "The name of your Azure DevOps organisation")]
        public string Organisation { get; set; }

        [Option(longName: "project", Required = true, HelpText = "The name of your Azure DevOps project")]
        public string Project { get; set; }

        [Option(longName: "token", Required = true, HelpText = "The personal access token required to authenticate with your Azure DevOps project")]
        public string PersonalAccessToken { get; set; }

        [Option(longName: "assembly", Required = true, HelpText = "The path to the assembly containing your UI tests")]
        public string AssemblyPath { get; set; }

        [Option(longName: "test-strategy", Required = true, HelpText = "The test runner used to execute your tests (i.e. SpecFlow+ Runner, NUnit etc.)")]
        public TestStrategy TestStratgey { get; set; }

        [Option(longName: "proxy", Required = false, HelpText = "The proxy URI if a web proxy is required")]
        public string ProxyHost { get; set; }

        [Option(longName: "port", Required = false, HelpText = "The proxy port")]
        public int ProxyPort { get; set; }

        [Option(longName: "username", Required = false, HelpText = "The proxy username if authentication credentials are required to authenticate with the proxy")]
        public string ProxyUsername { get; set; }

        [Option(longName: "password", Required = false, HelpText = "The proxy password if authentication credentials are required to authenticate with the proxy")]
        public string ProxyPassword { get; set; }
    }
}