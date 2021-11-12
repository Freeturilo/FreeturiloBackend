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
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace FreeturiloWebApi
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void Seed(FreeturiloContext _context)
        {
            _context.Administrators.AddRange(
                 new Administrator() { Id = 1, Name = "Miko�aj", Surname = "Ryll", Email = "mikolajryll@gmail.com", NotifyThreshold = 1, PasswordHash = "5f4dcc3b5aa765d61d8327deb882cf99" }
                 );

            _context.Stations.AddRange(
                new Station() { Id = 1, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Trasa �azienkowska x Marsza�kowska", Reports = 0, Lat = 52, Lon = 48 },
                new Station() { Id = 2, AvailableBikes = 10, AvailableRacks = 15, State = 1, Name = "Metro Wierzbno", Reports = 1, Lat = 52, Lon = 48 },
                new Station() { Id = 3, AvailableBikes = 10, AvailableRacks = 15, State = 2, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52, Lon = 48 }
                );

            _context.Routes.AddRange(
                new Route() { Id = 1, Cost = 10, RouteJSON = "", StartId = 1, StopId = 2, Time = 12 },
                new Route() { Id = 2, Cost = 10, RouteJSON = "", StartId = 1, StopId = 3, Time = 12 },
                new Route() { Id = 3, Cost = 10, RouteJSON = "", StartId = 2, StopId = 1, Time = 12 },
                new Route() { Id = 4, Cost = 10, RouteJSON = "", StartId = 2, StopId = 3, Time = 12 },
                new Route() { Id = 5, Cost = 10, RouteJSON = "", StartId = 3, StopId = 1, Time = 12 },
                new Route() { Id = 6, Cost = 10, RouteJSON = "", StartId = 3, StopId = 2, Time = 12 }
                );

            _context.SaveChanges();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAppService, AppService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<FreeturiloContext>(options => options.UseInMemoryDatabase("TestStartUpDB"));
            
            services.AddScoped<JwtMiddleware>();
            services.AddScoped<ExceptionHandlingMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeturiloWebApi v1"));
            }
           
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

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<FreeturiloContext>())
            {
                Seed(context);
            }
        }
    }
}