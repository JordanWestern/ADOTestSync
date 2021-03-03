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