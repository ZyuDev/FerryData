using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FerryData.Engine;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace FerryData.IS4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonoUserLoginController : ControllerBase
    {
        Serilog.ILogger _logger;
        IFerryApplication _ferryApp;
        AuthenticationStateProvider _AuthenticationStateProviderInstance;
        public MonoUserLoginController(Serilog.ILogger logger, IFerryApplication ferryApp, AuthenticationStateProvider AuthenticationStateProviderInstance)
        {
            _logger = logger;
            _ferryApp = ferryApp;
            _AuthenticationStateProviderInstance = AuthenticationStateProviderInstance;
        }



        [HttpGet("GetLoginStatus")]
        public bool Get()
        {
            /*
            MonoUserStatusReplyMessage m = new MonoUserStatusReplyMessage { IsLoggedIn = true };

            var json = JsonConvert.SerializeObject(m, Formatting.Indented,
                    new  JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            
            return Ok(json);
            */

            return _ferryApp.MonoUserLoggedIn;
        }

        [HttpPost("SetLoginStatus/{isLoggedIn}")]
        public void Set(string isLoggedIn)
        {
            _logger.Debug($"SetLoginStatus, incoming value= {isLoggedIn}");
            _ferryApp.MonoUserLoggedIn = (isLoggedIn=="True");
            RICOMPANY.CommonFunctions.ObjectParametersEngine.ObjectParameters.setObjectParameter(_AuthenticationStateProviderInstance, "IsLoggedIn", isLoggedIn);
        }

    }


}
