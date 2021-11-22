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
    public class NextBikeApiHandler: INextBikeApiHandler
    {
        private const string _dumpsPath = @"./xml-dumps/";
        private const string UrlBase = @"https://nextbike.net/maps/nextbike-live.xml";
        private const string UrlParameters = "?city=210";

        public markers GetNextBikeData(ILogger logger, bool readFromDump, string xsdPath, string dumpPath = null, string url = null, string parameters = null)
        {
            string xmlContent;

            if(readFromDump)
            {
                var dumpFilePath = Directory.GetFiles(dumpPath ?? _dumpsPath).Last();
                xmlContent = File.ReadAllText(dumpFilePath);
            }
            else
            {
                try
                {
                    HttpResponseMessage nextBikeResponse;
                    using (HttpClient client = new() { BaseAddress = new Uri(url ?? UrlBase) })
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        nextBikeResponse = client.GetAsync(parameters ?? UrlParameters).Result;
                    }

                    xmlContent = nextBikeResponse.Content.ReadAsStringAsync().Result;
                }
                catch
                {
                    logger.LogInformation("Could not get data from NextBike API");
                    return null;
                }
            }

            var nextBikeData = Parser.ReadNextBikesData(xmlContent, xsdPath);
            return nextBikeData;
            
        }
    }
}
