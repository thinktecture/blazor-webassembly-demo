using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ConfTool.Shared.Contracts;
using ConfTool.Shared.DTO;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;

namespace ConfTool.Modules.Conferences.Services
{
    public class ConferencesServiceClientGrpc : IConferencesServiceClient
    {
        private IConferencesService _client;
        //private Conference.Conferences.ConferencesClient _client;
        private HttpClient _anonHttpClient;

        private IConfiguration _config;
        private string _baseUrl;
        private string _statisticsUrl;
        private HubConnection _hubConnection;

        public event EventHandler ConferenceListChanged;

        public ConferencesServiceClientGrpc(IConfiguration config, GrpcChannel channel, CallInvoker invoker, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _baseUrl = _config[Configuration.BackendUrlKey];
            _statisticsUrl = new Uri(new Uri(_baseUrl), "api/statistics/").ToString();

            _anonHttpClient = httpClientFactory.CreateClient("Conferences.ServerAPI.Anon");

            //_client = new Conference.Conferences.ConferencesClient(channel);
            _client = channel.CreateGrpcService<IConferencesService>();
            //_client = GrpcClientFactory.CreateGrpcService<IConferencesService>(invoker);
        }

        public async Task InitAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri(new Uri(_baseUrl), "conferencesHub"))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("NewConferenceAdded", () =>
            {
                ConferenceListChanged?.Invoke(this, null);
            });

            await _hubConnection.StartAsync();
        }

        public async Task<List<ConferenceOverview>> ListConferencesAsync()
        {
            var result = await _client.ListConferencesAsync();

            return result.ToList();
        }

        public async Task<ConferenceDetails> GetConferenceDetailsAsync(Guid id)
        {
            var result = await _client.GetConferenceDetailsAsync(new ConferenceDetailsRequest { ID = id });

            return result;
        }

        public async Task<ConferenceDetails> AddConferenceAsync(ConferenceDetails conference)
        {
            var result = await _client.AddNewConferenceAsync(conference);

            return result;
        }

        public async Task<dynamic> GetStatisticsAsync()
        {
            var result = await _anonHttpClient.GetFromJsonAsync<dynamic>(_statisticsUrl);

            return result;
         }
    }
}
