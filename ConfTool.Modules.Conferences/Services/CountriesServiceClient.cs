using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConfTool.Modules.Conferences.Services
{
    public class CountriesServiceClient
    {
        private IConfiguration _config;
        private HttpClient _anonHttpClient;
        private string _baseUrl;
        private string _countriesUrl;

        public CountriesServiceClient(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _anonHttpClient = httpClientFactory.CreateClient("ConfTool.ServerAPI.Anon");
            _baseUrl = _config["BackendUrl"];
            _countriesUrl = new Uri(new Uri(_baseUrl), "api/countries/").ToString();
        }

        public async Task<List<string>> ListCountries()
        {
            var result = await _anonHttpClient.GetFromJsonAsync<List<string>>(_countriesUrl);

            return result;
        }
    }
}
