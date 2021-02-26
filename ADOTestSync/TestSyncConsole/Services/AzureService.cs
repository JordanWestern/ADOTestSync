using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;
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
                new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", launchSettings.Properties.PersonalAccessToken))));
        }

        public async Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage)
        {
            return JsonConvert.DeserializeObject<WorkItemQueryResult>(
                await this.httpClient.PostAsJsonAsync("{organization}/{project}/{team}/_apis/wit/wiql?api-version=6.0", workItemQueryLanguage.Query).Result.Content.ReadAsStringAsync());
        }

        private HttpClientHandler ConfigureClientHandler(ILaunchSettings launchSettings)
        {
            var clientHandler = new HttpClientHandler();

            if (launchSettings.Properties.ProxyHost != null && launchSettings.Properties.ProxyPort != 0)
            {
                clientHandler.Proxy = new WebProxy(launchSettings.Properties.ProxyHost, launchSettings.Properties.ProxyPort);
            }

            if (launchSettings.Properties.ProxyUsername != null && launchSettings.Properties.ProxyPassword != null)
            {
                clientHandler.Credentials = new NetworkCredential(launchSettings.Properties.ProxyUsername, launchSettings.Properties.ProxyPassword);
            }

            return clientHandler;
        }
    }
}
