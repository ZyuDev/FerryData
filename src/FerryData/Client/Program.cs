using BBComponents.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using FerryData.Engine;
using RICOMPANY.CommonFunctions.Logger;
using FerryData.Client;
using System.Diagnostics;
using Serilog;
using Serilog.Debugging;

namespace FerryData.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            /*
            builder.Services.AddHttpClient("FerryData.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FerryData.Server"));

            */

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Service to add alerts.
            builder.Services.AddScoped<IAlertService, AlertService>();

            FerryApplicationSettings ferryAppSettings = FerryApplicationSettings.GetMyInstance();

            builder.Services.AddSingleton<IFerryApplicationSettings>(x=>FerryApplicationSettings.GetMyInstance());

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.BrowserConsole()
                .CreateLogger();

            //Log.Debug("Hello, browser!");

            if (ferryAppSettings.MonoUser)
            {
                builder.Services.AddSingleton<AuthenticationStateProvider>(x => CustomAuthStateProvider.GetMyInstance(Log.Logger));             
            }

                builder.Services.AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = "https://auth.ricompany.info/";
                    options.ProviderOptions.ClientId = "client_blazor";

                    options.ProviderOptions.ResponseType = "code";

                    options.ProviderOptions.DefaultScopes.Add("profile");
                    options.ProviderOptions.DefaultScopes.Add("openid");
                    options.ProviderOptions.DefaultScopes.Add("Blazor");
                    options.ProviderOptions.DefaultScopes.Add("ServerAPI");

                    options.UserOptions.NameClaim = "preferred_username";
                });


            await builder.Build().RunAsync();
        }
    }
}
