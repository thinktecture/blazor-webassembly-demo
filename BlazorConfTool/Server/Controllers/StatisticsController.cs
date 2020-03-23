using Microsoft.AspNetCore.Mvc;
using BlazorConfTool.Server.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlazorConfTool.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StatisticsController : ControllerBase
    {
        private readonly ConferencesDbContext _conferencesDbContext;

        public StatisticsController(
            ConferencesDbContext conferencesDbContext)
        {
            _conferencesDbContext = conferencesDbContext;
        }

        [HttpGet]
        public async Task<dynamic> Get()
        {
            var conferences = await _conferencesDbContext.Conferences.ToListAsync();
            var result = conferences.GroupBy(a => a.Country).Select(
                conf => new { name = conf.Key, value = conf.Count() });

            return result;
        }
    }
}
