using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using RICOMPANY.CommonFunctions.Logger;
using RICOMPANY.CommonFunctions;

namespace FerryData.IS4
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var logger = new DynamicLogger("ri_auth_logs.txt", "Logger1", LogDirectionEnum.bothToConAndFile);
            
            logger.log("Point 1");

            var host = CreateHostBuilder(args).Build();
            
            logger.log("Point 2");

            CreateDbIfNotExists(host);
            logger.log("Point 3");
            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();

            try
            {
                DbInitializer.Initialize(scope);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
