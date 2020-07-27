using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ConfTool.Server.Hubs;
using ConfTool.Server.Model;
using ConfTool.Shared.Contracts;
using ConfTool.Shared.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConfTool.Server.gRPC
{
    public class ConferencesService : IConferencesService
    {
        private readonly ILogger<ConferencesService> _logger;
        private readonly ConferencesDbContext _conferencesDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<ConferencesHub> _hubContext;

        public ConferencesService(ILogger<ConferencesService> logger, ConferencesDbContext conferencesDbContext, IMapper mapper, IHubContext<ConferencesHub> hubContext)
        {
            _logger = logger;
            _conferencesDbContext = conferencesDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ConferenceDetails> AddNewConferenceAsync(ConferenceDetails conference)
        {
            var conf = _mapper.Map<Model.Conference>(conference);
            _conferencesDbContext.Conferences.Add(conf);
            await _conferencesDbContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("NewConferenceAdded");

            return _mapper.Map<Shared.DTO.ConferenceDetails>(conf);

        }

        public async Task<ConferenceDetails> GetConferenceDetailsAsync(ConferenceDetailsRequest request)
        {
            var conferenceDetails = await _conferencesDbContext.Conferences.FindAsync(request.ID);

            if (conferenceDetails == null)
            {
                return null;
            }

            return _mapper.Map<Shared.DTO.ConferenceDetails>(conferenceDetails);
        }

        public async Task<IEnumerable<Shared.DTO.ConferenceOverview>> ListConferencesAsync()
        {
            var conferences = await _conferencesDbContext.Conferences.ToListAsync();
            var confs = _mapper.Map<IEnumerable<Shared.DTO.ConferenceOverview>>(conferences);

            return confs;
        }
    }
}
