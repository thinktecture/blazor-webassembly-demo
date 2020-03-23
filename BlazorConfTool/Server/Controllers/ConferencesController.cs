using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorConfTool.Server.Model;
using AutoMapper;
using System.Threading.Tasks;
using BlazorConfTool.Shared;
using Microsoft.AspNetCore.SignalR;
using BlazorConfTool.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlazorConfTool.Server.Controllers
{
    [Authorize("api")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ConferencesController : ControllerBase
    {
        private readonly ILogger<ConferencesController> _logger;
        private readonly ConferencesDbContext _conferencesDbContext;
        private readonly IMapper _mapper;
        private readonly ConferenceDetailsValidator _validator;
        private readonly IHubContext<ConferencesHub> _hubContext;

        public ConferencesController(ILogger<ConferencesController> logger, 
            ConferencesDbContext conferencesDbContext, 
            IMapper mapper,
            ConferenceDetailsValidator validator,
            IHubContext<ConferencesHub> hubContext)
        {
            _logger = logger;
            _conferencesDbContext = conferencesDbContext;
            _mapper = mapper;
            _validator = validator;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Shared.DTO.ConferenceOverview>> Get()
        {
            var conferences = await _conferencesDbContext.Conferences.ToListAsync();

            return _mapper.Map<IEnumerable<Shared.DTO.ConferenceOverview>>(conferences);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shared.DTO.ConferenceDetails>> Get(string id)
        {
            var conferenceDetails = await _conferencesDbContext.Conferences.FindAsync(Guid.Parse(id));

            if (conferenceDetails == null)
            {
                return NotFound();
            }

            return _mapper.Map<Shared.DTO.ConferenceDetails>(conferenceDetails);
        }

        [HttpPost]
        public async Task<ActionResult<Shared.DTO.ConferenceDetails>> PostConference(Shared.DTO.ConferenceDetails conference)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var conf = _mapper.Map<Conference>(conference);
            _conferencesDbContext.Conferences.Add(conf);
            await _conferencesDbContext.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("NewConferenceAdded");

            return CreatedAtAction("Get", new { id = conference.ID }, _mapper.Map<Shared.DTO.ConferenceDetails>(conf));
        }
    }
}
