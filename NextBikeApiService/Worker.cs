using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NextBikeDataParser;

using NextBikeApiService.Helpers;

namespace NextBikeApiService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const int SecondsDelay = 3;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var token = FreeturiloApiHandler.GetToken(stoppingToken);
                var markers = NextBikeApiHandler.GetNextBikeData(stoppingToken, _logger);
                var freeturiloStations = FreeturiloApiHandler.GetFreeturiloStations(stoppingToken);

                await Task.Delay(1000 * SecondsDelay, stoppingToken);
            }
        }
    }
}
