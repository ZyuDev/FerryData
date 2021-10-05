using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using FerryData.Server.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using FerryData.Engine;
using Serilog;
using Serilog.Events;
using System.Text;

namespace FerryData.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            Serilog.Log.Information("ConfigureServices...");

            FerryApplicationSettings ferryAppSettings = FerryApplicationSettings.GetMyInstance();

            services.AddSingleton<Serilog.ILogger>(x => new LoggerConfiguration()
                                          .MinimumLevel.Debug()
                                          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                          .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                          .WriteTo.File("FerryLogs.txt")
                                          .CreateLogger());

            services.AddSingleton<IFerryApplication>(FerryApplication.GetMyInstance());

            //добавляем аутентификацию. Если моноюзер, то аутентиф. своя собственная. если нет, то идем на внешний IS4.
            Serilog.Log.Information($"Monouser={ferryAppSettings.MonoUser}");


            if (ferryAppSettings.MonoUser)
            {
                services.AddSingleton<AuthenticationStateProvider>(x => CustomAuthStateProvider.GetMyInstance(Log.Logger));

                //services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            }
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
                {
                    config.Authority = "https://auth.ricompany.info/";
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                    config.RequireHttpsMetadata = false;

                });
            
            
            services.AddCors(confg =>
               confg.AddPolicy("AllowAll",
                   p => p.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader()));

            services.AddControllersWithViews();
            services.AddRazorPages();

         //   var mongoConnectionString = Configuration.GetConnectionString("MongoDBConnectionString");

            //Сервис работы с базой Монго для Workflow
        //    services.AddSingleton<IWorkflowSettingsServiceAsync>(new WorkflowSettingsDbServiceAsync(mongoConnectionString));

            // Workflow settings service.
             services.AddSingleton<IWorkflowSettingsService>(new WorkflowSettingsInMemoryService());

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

         //   app.UseAuthentication();
          //  app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
    /*
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
    */
}
