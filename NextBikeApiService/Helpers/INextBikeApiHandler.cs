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
    public interface INextBikeApiHandler
    {
        /// <summary>
        /// Method to get data from NextBike
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="readFromDump"></param>
        /// <param name="xsdPath"></param>
        /// <param name="dumpPath"></param>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public markers GetNextBikeData(ILogger logger, bool readFromDump, string xsdPath = null, string dumpPath = null, string url = null, string parameters = null);
    }
}
