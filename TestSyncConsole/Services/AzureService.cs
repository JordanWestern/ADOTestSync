namespace TestSyncConsole.Services
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class AzureService : IAzureService
    {
        private readonly HttpClient httpClient;

        public AzureService(ILaunchSettings launchSettings)
        {
            var clientHandler = this.ConfigureClientHandler(launchSettings);

            this.httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri("https://dev.azure.com/")
            };

            this.httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json-patch+json"));

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", string.Empty, launchSettings.Arguments.PersonalAccessToken))));
        }

        // Need to see why this fails when value is a json patch...
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
        {
            var result = await this.httpClient.PostAsJsonAsync(requestUri, value);
            result.EnsureSuccessStatusCode();
            return result;
        }

        // Workaround for now until a solution is found for the above ^^
        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent value)
        {
            var result = await this.httpClient.PostAsync(requestUri, value);
            result.EnsureSuccessStatusCode();
            return result;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            var result = await this.httpClient.DeleteAsync(requestUri);
            result.EnsureSuccessStatusCode();
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