using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorConfTool.Client.Services
{
    public class CountriesService
    {
        private HttpClient _httpClient;
        private string _countriesUrl = "https://localhost:44323/api/countries/";

        public CountriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> ListCountries()
        {
            var result = await _httpClient.GetJsonAsync<List<string>>(_countriesUrl);

            return result;
        }
    }
}
