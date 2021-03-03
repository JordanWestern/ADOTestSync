namespace TestSyncConsole
{
    using CommandLine;

    public class LaunchSettings : ILaunchSettings
    {
        public LaunchSettings(string[] args)
        {
            Parser.Default.ParseArguments<Arguments>(args).WithParsed(model => { this.Arguments = model; });
        }

        public IArguments Arguments { get; private set; }
    }
}