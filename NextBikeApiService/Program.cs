using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FreeturiloWebApi.HttpMethods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NextBikeApiService.Helpers;

namespace NextBikeApiService
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// Creates host builder with required dependencies
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => 
                {
                    services.AddSingleton<IAppMethods, AppMethods>();
                    services.AddSingleton<IStationMethods, StationMethods>();
                    services.AddSingleton<IUserMethods, UserMethods>();
                    services.AddSingleton<INextBikeApiHandler, NextBikeApiHandler>();
                    services.AddSingleton<IWorkerHandler, WorkerHandler>();
                    services.AddHostedService<Worker>();
                });
    }
}