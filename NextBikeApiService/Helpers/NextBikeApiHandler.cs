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
        /// <summary>
        /// Default path to folder with NextBike data dumps
        /// </summary>
        private const string _dumpsPath = @"./xml-dumps/";

        /// <summary>
        /// URL to NextBike API
        /// </summary>
        private const string UrlBase = @"https://nextbike.net/maps/nextbike-live.xml";

        /// <summary>
        /// Parameters to get data about bikes
        /// </summary>
        private const string UrlParameters = "?city=210";

        /// <summary>
        /// Method to get data from NextBike API or from dumps directory 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="readFromDump"></param>
        /// <param name="xsdPath"></param>
        /// <param name="dumpPath"></param>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
