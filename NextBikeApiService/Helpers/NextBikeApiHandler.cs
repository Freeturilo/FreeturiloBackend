using NextBikeDataParser;
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

        public static markers GetNextBikeData(ILogger logger, bool readFromDump, CancellationToken stoppingToken)
        {
            string xmlContent;

            if(readFromDump)
            {
                var dumpFilePath = Directory.GetFiles(_dumpsPath).Last();
                xmlContent = File.ReadAllText(dumpFilePath);
            }
            else
            {
                try
                {
                    HttpResponseMessage nextBikeResponse;
                    using (HttpClient client = new() { BaseAddress = new Uri(UrlBase) })
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        nextBikeResponse = client.GetAsync(UrlParameters, stoppingToken).Result;
                    }

                    xmlContent = nextBikeResponse.Content.ReadAsStringAsync(stoppingToken).Result;
                }
                catch
                {
                    logger.LogInformation("Could not get data from NextBike API");
                    return null;
                }
            }

            var nextBikeData = Parser.ReadNextBikesData(xmlContent);
            return nextBikeData;
            
        }
    }
}
