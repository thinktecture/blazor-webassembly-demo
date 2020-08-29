using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfTool.Shared.DTO;

namespace ConfTool.Client.Services
{
    public interface IConferencesServiceClient
    {
        Task InitAsync();
        Task<List<ConferenceOverview>> ListConferencesAsync();
        Task<ConferenceDetails> AddConferenceAsync(ConferenceDetails conference);
        Task<ConferenceDetails> GetConferenceDetailsAsync(Guid id);
        Task<dynamic> GetStatisticsAsync();

        event EventHandler ConferenceListChanged;
    }
}