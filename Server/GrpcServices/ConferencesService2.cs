using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Conference;
using ConfTool.Server.Model;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConfTool.Server.gRPC
{
    public class ConferencesService2 : Conferences.ConferencesBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly ConferencesDbContext _conferencesDbContext;
        private readonly IMapper _mapper;

        public ConferencesService2(ILogger<GreeterService> logger, ConferencesDbContext conferencesDbContext, IMapper mapper)
        {
            _logger = logger;
            _conferencesDbContext = conferencesDbContext;
            _mapper = mapper;
        }

        public override async Task<ListConferencesResponse> ListConferences(ListConferencesRequest request, ServerCallContext context)
        {
            var conferences = await _conferencesDbContext.Conferences.ToListAsync();
            var confs = _mapper.Map<IEnumerable<ConferenceOverview>>(conferences);

            var response = new ListConferencesResponse();
            response.Conferences.AddRange(confs);

            return response;
        }
    }
}
