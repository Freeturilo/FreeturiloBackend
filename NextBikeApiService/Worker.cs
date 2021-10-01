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
using Microsoft.AspNetCore.SignalR.Client;
using FreeturiloWebApi.Models;

namespace NextBikeApiService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _xsdPath = Environment.GetEnvironmentVariable("XSD_PATH") ??  @"../NextBikeDataParser/markers.xsd";
        private readonly string _hubPath = Environment.GetEnvironmentVariable("HUB_PATH") ??  @"http://localhost:5000/nextBike";

        private const string LibUrl = @"http://example.org/mr/nextbikesdata";
        private const string UrlBase = @"https://nextbike.net/maps/nextbike-live.xml";
        private const string UrlParameters = "?city=210";

        private const int SecondsDelay = 3;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _connection = new HubConnectionBuilder()
                .WithUrl(_hubPath)
                .Build();
            await _connection.StartAsync();
            _logger.LogInformation("Connected to Hub on " + _hubPath);

            while (!stoppingToken.IsCancellationRequested)
            {
                HttpResponseMessage response;
                using (HttpClient client = new() { BaseAddress = new Uri(UrlBase) })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    response = client.GetAsync(UrlParameters, stoppingToken).Result;
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error when requesting nextbike.net: {response.StatusCode} {response.Content}");
                }

                var xmlContent = response.Content.ReadAsStringAsync(stoppingToken).Result;
                var xmlContentSplit = xmlContent.Split(new string[] { "markers" }, StringSplitOptions.None);
                var xmlContentJoined = xmlContentSplit[0] + $"markers xmlns=\"{LibUrl}\"" + xmlContentSplit[1] +
                                       "markers" +
                                       xmlContentSplit[2];
                var nextBikesData = Parser.ReadNextBikesData(xmlContentJoined, _xsdPath, LibUrl);

                var stations = new List<Station>();
                foreach (var place in nextBikesData.country.city.place)
                {
                    var station = new Station()
                    {
                        Id = place.uid,
                        Lng = (double)place.lng,
                        Lat = (double)place.lat,
                        Name = place.name,
                        AvailableBikes = place.bikes_available_to_rent,
                    };

                    stations.Add(station);
                }

                await _connection.InvokeAsync("UpdateStations", stations);
                _logger.LogInformation("Status updated");
                await Task.Delay(1000 * SecondsDelay, stoppingToken);
            }
        }
    }
}