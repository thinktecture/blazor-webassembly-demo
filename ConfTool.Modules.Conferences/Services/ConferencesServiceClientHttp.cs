using ConfTool.Shared.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConfTool.Modules.Conferences.Services
{
    public class ConferencesServiceClientHttp : IConferencesServiceClient
    {
        private IConfiguration _config;
        private HttpClient _secureHttpClient;
        private HttpClient _anonHttpClient;
        private string _baseUrl;
        private string _conferencesUrl;
        private string _statisticsUrl;
        private HubConnection _hubConnection;

        public event EventHandler ConferenceListChanged;

        public ConferencesServiceClientHttp(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _secureHttpClient = httpClientFactory.CreateClient("Conferences.ServerAPI");
            _anonHttpClient = httpClientFactory.CreateClient("Conferences.ServerAPI.Anon");
            _baseUrl = _config[Configuration.BackendUrlKey];
            _conferencesUrl = new Uri(new Uri(_baseUrl), "api/conferences/").ToString();
            _statisticsUrl = new Uri(new Uri(_baseUrl), "api/statistics/").ToString();
        }

        public async Task InitAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(_baseUrl), "conferencesHub"))
                .Build();

            _hubConnection.On("NewConferenceAdded", () =>
            {
                ConferenceListChanged?.Invoke(this, null);
            });

            await _hubConnection.StartAsync();
        }

        public async Task<List<ConferenceOverview>> ListConferencesAsync()
        {
            var result = await _secureHttpClient.GetFromJsonAsync<List<ConferenceOverview>>(_conferencesUrl);

            return result;
        }

        public async Task<ConferenceDetails> GetConferenceDetailsAsync(Guid id)
        {
            var result = await _secureHttpClient.GetFromJsonAsync<ConferenceDetails>(_conferencesUrl + id);

            return result;
        }

        public async Task<ConferenceDetails> AddConferenceAsync(ConferenceDetails conference)
        {
            var result = await (await _secureHttpClient.PostAsJsonAsync<ConferenceDetails>(_conferencesUrl, conference))
                .Content.ReadFromJsonAsync<ConferenceDetails>();

            return result;
        }

        public async Task<dynamic> GetStatisticsAsync()
        {
            var result = await _anonHttpClient.GetFromJsonAsync<dynamic>(_statisticsUrl);

            return result;
        }
    }
}
