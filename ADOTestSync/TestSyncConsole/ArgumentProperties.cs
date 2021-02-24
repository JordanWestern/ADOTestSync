using CommandLine;

namespace TestSyncConsole
{
    public class ArgumentProperties
    {
        [Option(shortName: 'o', longName: "organisation", Required = true, HelpText = "The name of your Azure DevOps organisation")]
        public string Organisation { get; set; }

        [Option(shortName: 'p', longName: "project", Required = true, HelpText = "The name of your Azure DevOps project")]
        public string Project { get; set; }

        [Option(shortName: 't', longName: "token", Required = true, HelpText = "The personal access token required to authenticate with your Azure DevOps project")]
        public string PersonalAccessToken { get; set; }
    }
}
