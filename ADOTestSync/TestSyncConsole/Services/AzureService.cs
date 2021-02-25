using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestSyncConsole.Services
{
    public class AzureService
    {
        private readonly HttpClient httpClient;
        private readonly IConsoleArguments consoleArguments;

        public AzureService(HttpClient httpClient, IConsoleArguments consoleArguments)
        {
            httpClient.BaseAddress = new Uri("https://dev.azure.com/");

            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", consoleArguments.Properties.PersonalAccessToken))));

            this.httpClient = httpClient;
            this.consoleArguments = consoleArguments;
        }

        public async Task<WorkItemQueryResult> PostWorkItemQueryAsync(Wiql workItemQueryLanguage)
        {
            return JsonConvert.DeserializeObject<WorkItemQueryResult>(
                await this.httpClient.PostAsJsonAsync("{organization}/{project}/{team}/_apis/wit/wiql?api-version=6.0", workItemQueryLanguage.Query).Result.Content.ReadAsStringAsync());
        }
    }
}
