using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NextBikeDataParser;

using NextBikeApiService.Helpers;
using FreeturiloWebApi.HttpMethods;

namespace NextBikeApiService
{
    public class Worker : BackgroundService
    {
        private static readonly string serverPath = @"https://localhost:5001/";

        private const string email = "freeturilo@gmail.com";
        private const string password = "Freeturilo123PW!";

        private readonly ILogger<Worker> _logger;
        private const int SecondsDelay = 5;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                (var markers, var _) = NextBikeApiHandler.GetNextBikeData(_logger, stoppingToken);
                var freeturiloStations = StationMethods.GetAllStations(serverPath);

                var globalUpdate = BikeDataComparer.Compare(markers, freeturiloStations, out var toBeAdded, out var toBeUpdated);

                var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });

                if(globalUpdate)
                {
                    toBeUpdated.AddRange(toBeAdded);
                    StationMethods.UpdateAllStations(serverPath, token, toBeUpdated.ToArray());
                    _logger.LogInformation("Global update: all stations updated");
                }
                else
                {
                    foreach(var station in toBeUpdated)
                    {
                        StationMethods.UpdateStation(serverPath, token, station.Id, station);
                        _logger.LogInformation($"Station updated: {station.Name}");
                    }

                    foreach (var station in toBeAdded)
                    {
                        StationMethods.AddNewStation(serverPath, token, station);
                        _logger.LogInformation($"New station added: {station.Name}");

                    }
                }

                await Task.Delay(1000 * SecondsDelay, stoppingToken);
            }
        }
    }
}
