using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.IS4.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        Serilog.ILogger _logger;
        public StatusController(Serilog.ILogger logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public string Get()
        {

            _logger.Information("Status reported");
            return "Hi, this is IDS4!";


        }
    }


}
