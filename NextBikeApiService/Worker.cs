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
using System.Linq;
using FreeturiloWebApi.DTO;
using System.Diagnostics.CodeAnalysis;

namespace NextBikeApiService
{
    [ExcludeFromCodeCoverage]
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Worker to launch
        /// </summary>
        private readonly IWorkerHandler _worker;
        
        /// <summary>
        /// Constant indicating worker interval
        /// </summary>
        private const int _secondsDelay = 5;
        
        public Worker(IWorkerHandler worker)
        {
            _worker = worker;
        }
        /// <summary>
        /// Methods launch worker
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _worker.Work();
                await Task.Delay(1000 * _secondsDelay, stoppingToken);
            }
        }
    }
}
