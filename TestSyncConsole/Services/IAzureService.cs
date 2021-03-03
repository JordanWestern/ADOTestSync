namespace TestSyncConsole.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IAzureService
    {
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value);
    }
}