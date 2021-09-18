using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NextBikeDataParser;
using StackExchange.Redis;

namespace FreeturiloRedisWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDatabase _database;
        private readonly string _redisEndpoint = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost:6379";
        private readonly string _xsdPath = Environment.GetEnvironmentVariable("XSD_PATH") ??  @"../NextBikeDataParser/markers.xsd";

        private const string LibUrl = @"http://example.org/mr/nextbikesdata";
        private const string UrlBase = @"https://nextbike.net/maps/nextbike-live.xml";
        private const string UrlParameters = "?city=210";

        private const int SecondsDelay = 20;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _database = ConnectionMultiplexer.Connect(
                new ConfigurationOptions()
                {
                    EndPoints = {_redisEndpoint}
                }).GetDatabase();
            _logger.LogInformation($"Connected to redis on host: {_redisEndpoint}");
            _logger.LogInformation($"XSD Path: {_xsdPath}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                HttpResponseMessage response;
                using (HttpClient client = new() {BaseAddress = new Uri(UrlBase)})
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    response = client.GetAsync(UrlParameters, stoppingToken).Result;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error when requesting nextbike.net: {response.StatusCode} {response.Content}");
                }

                var xmlContent = response.Content.ReadAsStringAsync(stoppingToken).Result;
                var xmlContentSplit = xmlContent.Split(new string[] {"markers"}, StringSplitOptions.None);
                var xmlContentJoined = xmlContentSplit[0] + $"markers xmlns=\"{LibUrl}\"" + xmlContentSplit[1] +
                                       "markers" +
                                       xmlContentSplit[2];
                var nextBikesData = Parser.ReadNextBikesData(xmlContentJoined, _xsdPath, LibUrl);

                foreach (var place in nextBikesData.country.city.place)
                {
                    string key = $"main:{place.name}:bikes";
                    string oldBikes = _database.StringGet(key);
                    string newBikes = place.bikes_available_to_rent.ToString();
                    if (oldBikes != newBikes)
                    {
                        _database.StringSet(key, newBikes);
                        _logger.LogInformation($"Changed value in redis: {key}: {oldBikes} -> {newBikes}");
                    }
                }
                
                await Task.Delay(1000 * SecondsDelay, stoppingToken);
            }
        }
    }
}