using System.Net.Http;
using System.Threading.Tasks;

namespace TestSyncConsole.Services
{
    public interface IAzureService
    {
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value);
    }
}