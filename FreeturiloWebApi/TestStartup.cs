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

        public int i = new Random().Next();

        private static void Seed(FreeturiloContext _context)
        {
            _context.Administrators.AddRange(
                 new Administrator() { Id = 1, Name = "Miko³aj", Surname = "Ryll", Email = "mikolajryll@gmail.com", NotifyThreshold = 1, PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8" }
                 );

            _context.Stations.AddRange(
                new Station() { Id = 1, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Trasa £azienkowska x Marsza³kowska", Reports = 0, Lat = 52.22, Lon = 20.97 },
                new Station() { Id = 2, AvailableBikes = 10, AvailableRacks = 15, State = 1, Name = "Metro Wierzbno", Reports = 1, Lat = 52.249512, Lon = 21.043405, },
                new Station() { Id = 6, AvailableBikes = 10, AvailableRacks = 15, State = 2, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52.290173, Lon = 20.95037 },
                new Station() { Id = 5, AvailableBikes = 0, AvailableRacks = 15, State = 0, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52.290173, Lon = 20.95037 },
                new Station() { Id = 3, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52.25, Lon = 21.04 },
                new Station() { Id = 4, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52.235, Lon = 21.005 },
                new Station() { Id = 7, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lat = 52.235, Lon = 21.005 },
                new Station() { Id = 8, AvailableBikes = 10, AvailableRacks = 15, State = 0, Name = "Dworzec Centralny x Emilii Plater", Reports = 10, Lon = 21.043405, Lat = 52.220877 }
                );

            _context.Routes.AddRange(
                new Route() { Id = 1, Cost = 10, RouteJSON = "", StartId = 1, StopId = 2, Time = 24 *60 },
                new Route() { Id = 2, Cost = 10, RouteJSON = "", StartId = 1, StopId = 3, Time = 5 * 60 },
                new Route() { Id = 3, Cost = 10, RouteJSON = "", StartId = 2, StopId = 1, Time = 24 * 60 },
                new Route() { Id = 4, Cost = 10, RouteJSON = "", StartId = 2, StopId = 3, Time = 5 * 60 },
                new Route() { Id = 5, Cost = 10, RouteJSON = "", StartId = 3, StopId = 1, Time = 5 * 60 },
                new Route() { Id = 6, Cost = 10, RouteJSON = "", StartId = 3, StopId = 2, Time = 5 * 60 },
                new Route() { Id = 7, Cost = 10, RouteJSON = "", StartId = 4, StopId = 1, Time = 1 * 60 },
                new Route() { Id = 8, Cost = 10, RouteJSON = "", StartId = 4, StopId = 2, Time = 100 * 60 },
                new Route() { Id = 9, Cost = 10, RouteJSON = "", StartId = 4, StopId = 3, Time = 100 * 60 },
                new Route() { Id = 10, Cost = 10, RouteJSON = "", StartId = 1, StopId = 4, Time = 100 * 60 },
                new Route() { Id = 11, Cost = 10, RouteJSON = "", StartId = 2, StopId = 4, Time = 1 * 60 },
                new Route() { Id = 12, Cost = 10, RouteJSON = "", StartId = 3, StopId = 4, Time = 100 * 60 },

                new Route() { Id = 13, Cost = 10, RouteJSON = "", StartId = 1, StopId = 7, Time = 1 * 60 },
                new Route() { Id = 14, Cost = 10, RouteJSON = "", StartId = 2, StopId = 7, Time = 1 * 60 },
                new Route() { Id = 15, Cost = 10, RouteJSON = "", StartId = 3, StopId = 7, Time = 1 * 60 },
                new Route() { Id = 16, Cost = 10, RouteJSON = "", StartId = 4, StopId = 7, Time = 1 * 60 },
                new Route() { Id = 17, Cost = 10, RouteJSON = "", StartId = 1, StopId = 8, Time = 100 * 60 },
                new Route() { Id = 18, Cost = 10, RouteJSON = "", StartId = 2, StopId = 8, Time = 100 * 60 },
                new Route() { Id = 19, Cost = 10, RouteJSON = "", StartId = 3, StopId = 8, Time = 100 * 60 },
                new Route() { Id = 20, Cost = 10, RouteJSON = "", StartId = 4, StopId = 8, Time = 100 * 60 },

                new Route() { Id = 21, Cost = 10, RouteJSON = "", StartId = 8, StopId = 1, Time = 100 * 60 },
                new Route() { Id = 22, Cost = 10, RouteJSON = "", StartId = 8, StopId = 2, Time = 100 * 60 },
                new Route() { Id = 23, Cost = 10, RouteJSON = "", StartId = 8, StopId = 3, Time = 100 * 60 },
                new Route() { Id = 24, Cost = 10, RouteJSON = "", StartId = 8, StopId = 4, Time = 100 * 60 },
                new Route() { Id = 25, Cost = 10, RouteJSON = "", StartId = 8, StopId = 7, Time = 21 * 60 },

                new Route() { Id = 26, Cost = 10, RouteJSON = "", StartId = 7, StopId = 1, Time = 100 * 60 },
                new Route() { Id = 27, Cost = 10, RouteJSON = "", StartId = 7, StopId = 2, Time = 100 * 60 },
                new Route() { Id = 28, Cost = 10, RouteJSON = "", StartId = 7, StopId = 3, Time = 100 * 60 },
                new Route() { Id = 29, Cost = 10, RouteJSON = "", StartId = 7, StopId = 4, Time = 100 * 60 },
                new Route() { Id = 30, Cost = 10, RouteJSON = "", StartId = 7, StopId = 8, Time = 21 * 60 }
                );

            _context.State.Add(
                new State() { Value = 0, Id = 1 }
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
            services.AddScoped<IRouteService, RouteService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<FreeturiloContext>(options =>
            {
                options.UseInMemoryDatabase("TestStartUpDB" + i);
            });
            
            services.AddScoped<JwtMiddleware>();
            services.AddScoped<ExceptionHandlingMiddleware>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {      
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<FreeturiloContext>();
            Seed(context);
        }
    }
}
