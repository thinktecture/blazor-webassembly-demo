
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfTool.Shared.Contracts;
using ConfTool.Shared.DTO;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using ProtoBuf.Grpc.Client;

namespace ConfTool.Client.Services
{
    public class ConferencesServiceGrpc : IConferencesServiceClient
    {
        private IConferencesService _client;
        //private Conference.Conferences.ConferencesClient _client;

        private IConfiguration _config;
        private string _baseUrl;
        private HubConnection _hubConnection;

        public event EventHandler ConferenceListChanged;

        public ConferencesServiceGrpc(IConfiguration config, CallInvoker invoker)
        {
            _config = config;
            _baseUrl = _config["BackendUrl"];

            //_client = new Conference.Conferences.ConferencesClient(channel);
            //_client = channel.CreateGrpcService<IConferencesService>();
            _client = GrpcClientFactory.CreateGrpcService<IConferencesService>(invoker);
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
            var result = await  _client.GetConferenceDetailsAsync(new ConferenceDetailsRequest { ID = id });

            return result;
        }

        public async Task<ConferenceDetails> AddConferenceAsync(ConferenceDetails conference)
        {
            var result = await _client.AddNewConferenceAsync(conference);

            return result;
        }

        public async Task<dynamic> GetStatisticsAsync()
        {
            /*
            var result = await _httpClient.GetFromJsonAsync<dynamic>(_statisticsUrl);

            return result;
            */
            throw new NotImplementedException();
        }
    }
}
