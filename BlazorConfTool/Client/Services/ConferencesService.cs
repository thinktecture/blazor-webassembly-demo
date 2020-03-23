using BlazorConfTool.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Sotsera.Blazor.Oidc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorConfTool.Client.Services
{
    public class ConferencesService
    {
        private OidcHttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:44323/";
        private string _conferencesUrl;
        private string _statisticsUrl;
        private HubConnection _hubConnection;

        public EventHandler ConferenceListChanged;

        public ConferencesService(OidcHttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var result = await _httpClient.GetJsonAsync<List<ConferenceOverview>>(_conferencesUrl);
            
            return result;
        }

        public async Task<ConferenceDetails> GetConferenceDetails(Guid id)
        {
            var result = await _httpClient.GetJsonAsync<ConferenceDetails>(_conferencesUrl + id);

            return result;
        }

        public async Task<ConferenceDetails> AddConference(ConferenceDetails conference)
        {
            var result = await _httpClient.PostJsonAsync<ConferenceDetails>(
                _conferencesUrl, conference);

            return result;
        }

        public async Task<dynamic> GetStatistics()
        {
            var result = await _httpClient.GetJsonAsync<dynamic>(_statisticsUrl);

            return result;
        }
    }
}
