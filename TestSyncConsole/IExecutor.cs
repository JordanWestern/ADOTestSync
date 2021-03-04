namespace TestSyncConsole
{
    using System.Threading.Tasks;

    public interface IExecutor
    {
        Task ExecuteAsync();
    }
}