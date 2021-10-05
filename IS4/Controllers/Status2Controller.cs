using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FerryData.IS4.Controllers
{
    [Route("/")]
    [ApiController]
    public class Status2Controller : ControllerBase
    {
        Serilog.ILogger _logger;
        public Status2Controller(Serilog.ILogger logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public string Get()
        {

            _logger.Information("Status2 reported");
            return "Hi, this is IDS4!_2";


        }
    }


}
