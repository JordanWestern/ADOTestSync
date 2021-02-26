using CommandLine;

namespace TestSyncConsole
{
    public class LaunchSettings : ILaunchSettings
    {
        public Arguments Properties { get; private set; }

        public LaunchSettings(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args).WithParsed(model => { this.Properties = model; });
        }
    }
}
