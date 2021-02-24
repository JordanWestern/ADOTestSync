using CommandLine;

namespace TestSyncConsole
{
    public class ConsoleArguments : IConsoleArguments
    {
        public ArgumentProperties Properties { get; private set; }

        public ConsoleArguments(string[] args)
        {
            Parser.Default.ParseArguments<ArgumentProperties>(args).WithParsed(model => { this.Properties = model; });
        }
    }
}
