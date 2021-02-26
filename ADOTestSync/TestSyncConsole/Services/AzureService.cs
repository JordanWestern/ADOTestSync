using Serilog;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestSyncConsole.Services
{
    public class AzureService : IAzureService
    {
        private readonly HttpClient httpClient;

        public AzureService(ILaunchSettings launchSettings)
        {
            var clientHandler = this.ConfigureClientHandler(launchSettings);

            this.httpClient = new HttpClient(clientHandler);

            httpClient.BaseAddress = new Uri("https://dev.azure.com/");

            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json-patch+json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", launchSettings.Arguments.PersonalAccessToken))));
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            var result =  await this.httpClient.PostAsJsonAsync(requestUri, value);

            try
            {
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Log.Logger.Error($"The request to {result.RequestMessage.RequestUri} failed: {e.Message}");
                throw e;
            }

            return result;
        }

        private HttpClientHandler ConfigureClientHandler(ILaunchSettings launchSettings)
        {
            var clientHandler = new HttpClientHandler();

            if (launchSettings.Arguments.ProxyHost != null && launchSettings.Arguments.ProxyPort != 0)
            {
                clientHandler.Proxy = new WebProxy(launchSettings.Arguments.ProxyHost, launchSettings.Arguments.ProxyPort);
            }

            if (launchSettings.Arguments.ProxyUsername != null && launchSettings.Arguments.ProxyPassword != null)
            {
                clientHandler.Credentials = new NetworkCredential(launchSettings.Arguments.ProxyUsername, launchSettings.Arguments.ProxyPassword);
            }

            return clientHandler;
        }
    }
}