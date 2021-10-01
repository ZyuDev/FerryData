using FerryData.Engine.Abstract.Service;
using FerryData.Engine.Environment;
using FerryData.Server.Data;
using FerryData.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
                {
                    config.Authority = "https://localhost:10001";

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

            services.Configure<MongoDatabaseSettings>(
             Configuration.GetSection(nameof(MongoDatabaseSettings)));

            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

            services.AddSingleton(typeof(IMongoService<>), typeof(MongoService<>));

            services.AddTransient<IDbInitializer, DbInitializer>();

            //enabling the in memory cache 
            services.AddMemoryCache();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddOpenApiDocument(options =>
            {
                options.Title = "FerryData API Doc";
                options.Version = "1.0";
            });
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IDbInitializer dbInitializer)
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

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            dbInitializer.InitializeDb();
        }
    }
}
