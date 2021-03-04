namespace TestSyncConsole.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IAzureService
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value);

        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent value);
    }
}