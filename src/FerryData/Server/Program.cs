using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FerryData.Engine;
using Serilog;
using Serilog.Events;

namespace FerryData.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //чтобы синглтон приложения был создан на старте
            IFerryApplication App = FerryApplication.GetMyInstance();

            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
              .WriteTo.File("FerryLogsStatic.txt")
              .CreateLogger();

            Serilog.Log.Information("Starting server app");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
