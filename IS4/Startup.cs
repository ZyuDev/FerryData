using FerryData.IS4.Data;
using FerryData.IS4.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FerryData.IS4
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IsDbContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection2"));
            });


            services.AddSingleton<Serilog.ILogger>(x => new LoggerConfiguration()
                                          .MinimumLevel.Debug()
                                          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                          .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                          .WriteTo.File("AppLog.txt")
                                          .CreateLogger());

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<IsDbContext>()
                .AddDefaultTokenProviders();

            
            
          //  services.AddSingleton(new DynamicLogger ("ri_auth_logs.txt", "Logger1", LogDirectionEnum.bothToConAndFile ));

            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Identification/Login";
                options.UserInteraction.LogoutUrl = "/Identification/Logout";
            })
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
                .AddInMemoryClients(IdentityServerConfiguration.GetClients())
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryApiScopes(IdentityServerConfiguration.GetScopes())
                .AddDeveloperSigningCredential();

            services.AddCors();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wa02 v1"));
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    
}
