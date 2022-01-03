using FreeturiloWebApi.Controllers;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Middlewares;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace FreeturiloWebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeturiloWebApi", Version = "v1" });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "FreeturiloWebApi.xml");
                c.IncludeXmlComments(filePath);
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IRouteService, RouteService>();

            /*services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
                });
            });*/

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<FreeturiloContext>(options =>
                options.UseNpgsql(_connectionString ?? Configuration.GetConnectionString("FreeturiloDatabase"))
            );
            
            services.AddScoped<JwtMiddleware>();
            services.AddScoped<ExceptionHandlingMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeturiloWebApi v1"));
           
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
