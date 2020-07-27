using ConfTool.Shared.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConfTool.Client.Services
{
    public class ConferencesService
    {
        private IConfiguration _config;
        private HttpClient _secureHttpClient;
        private HttpClient _anonHttpClient;
        private string _baseUrl;
        private string _conferencesUrl;
        private string _statisticsUrl;
        private HubConnection _hubConnection;

        public EventHandler ConferenceListChanged;

        public ConferencesService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _secureHttpClient = httpClientFactory.CreateClient("ConfTool.ServerAPI");
            _anonHttpClient = httpClientFactory.CreateClient("ConfTool.ServerAPI.Anon");
            _baseUrl = _config["BackendUrl"];
            _conferencesUrl = new Uri(new Uri(_baseUrl), "api/conferences/").ToString();
            _statisticsUrl = new Uri(new Uri(_baseUrl), "api/statistics/").ToString();
        }

        public async Task Init()
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

        public async Task<List<ConferenceOverview>> ListConferences()
        {
            var result = await _secureHttpClient.GetFromJsonAsync<List<ConferenceOverview>>(_conferencesUrl);
            
            return result;
        }

        public async Task<ConferenceDetails> GetConferenceDetails(Guid id)
        {
            var result = await _secureHttpClient.GetFromJsonAsync<ConferenceDetails>(_conferencesUrl + id);

            return result;
        }

        public async Task<ConferenceDetails> AddConference(ConferenceDetails conference)
        {
            var result = await (await _secureHttpClient.PostAsJsonAsync<ConferenceDetails>(_conferencesUrl, conference))
                .Content.ReadFromJsonAsync<ConferenceDetails>();

            return result;
        }

        public async Task<dynamic> GetStatistics()
        {
            var result = await _anonHttpClient.GetFromJsonAsync<dynamic>(_statisticsUrl);

            return result;
        }
    }
}
