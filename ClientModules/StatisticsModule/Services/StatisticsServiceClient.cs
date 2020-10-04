using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConfTool.ClientModules.Statistics.Services
{
    public class StatisticsServiceClient
    {
        private IConfiguration _config;
        private HttpClient _anonHttpClient;
        private string _baseUrl;
        private string _statisticsUrl;

        public StatisticsServiceClient(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _anonHttpClient = httpClientFactory.CreateClient("Statistics.ServerAPI.Anon");
            _baseUrl = _config[Configuration.BackendUrlKey];
            _statisticsUrl = new Uri(new Uri(_baseUrl), "api/statistics/").ToString();
        }

        public async Task<dynamic> GetStatisticsAsync()
        {
            var result = await _anonHttpClient.GetFromJsonAsync<dynamic>(_statisticsUrl);

            return result;
        }
    }
}
