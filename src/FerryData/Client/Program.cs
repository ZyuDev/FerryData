using BBComponents.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Service to add alerts.
            builder.Services.AddScoped<IAlertService, AlertService>();

            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = "https://localhost:10001";
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
