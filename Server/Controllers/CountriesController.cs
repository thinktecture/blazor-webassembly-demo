using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorConfTool.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CountriesController : ControllerBase
    {
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ILogger<CountriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var countries = new List<string>(){
                "Germany",
                "Austria",
                "Switzerland",
                "Belgium",
                "Netherlands",
                "USA",
                "England"
            };

            return countries;
        }
    }
}
