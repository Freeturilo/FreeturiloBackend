﻿using NextBikeDataParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.IO;

namespace NextBikeApiService.Helpers
{
    static class NextBikeApiHandler
    {
        private static readonly string _xsdPath = @"../NextBikeDataParser/markers.xsd";
        private static readonly string _dumpsPath = @"./xml-dumps/";

        private const string LibUrl = @"http://example.org/mr/nextbikesdata";
        private const string UrlBase = @"https://nextbike.net/maps/nextbike-live.xml";
        private const string UrlParameters = "?city=210";

        public static (markers, bool) GetNextBikeData(ILogger logger, CancellationToken stoppingToken)
        {
            try
            {
                HttpResponseMessage nextBikeResponse;
                using (HttpClient client = new() { BaseAddress = new Uri(UrlBase) })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    nextBikeResponse = client.GetAsync(UrlParameters, stoppingToken).Result;
                }

                string xmlContent;
                bool isNextbikeDataAvailable = true;

                if (!nextBikeResponse.IsSuccessStatusCode)
                {
                    logger.LogError($"Error when requesting nextbike.net: {nextBikeResponse.StatusCode} {nextBikeResponse.Content}");
                    logger.LogWarning($"Data from XML dumps");

                    var dumpFilePath = Directory.GetFiles(_dumpsPath).Last();
                    xmlContent = File.ReadAllText(dumpFilePath);
                    
                    isNextbikeDataAvailable = false;
                }
                else
                {
                    xmlContent = nextBikeResponse.Content.ReadAsStringAsync(stoppingToken).Result;
                }

                var xmlContentSplit = xmlContent.Split(new string[] { "markers" }, StringSplitOptions.None);
                var xmlContentJoined = xmlContentSplit[0] + $"markers xmlns=\"{LibUrl}\"" + xmlContentSplit[1] +
                                       "markers" +
                                       xmlContentSplit[2];
                var nextBikeData = Parser.ReadNextBikesData(xmlContentJoined, _xsdPath, LibUrl);
                return (nextBikeData, isNextbikeDataAvailable);
            }
            catch
            {
                return (null, false);
            }
            
        }
    }
}
