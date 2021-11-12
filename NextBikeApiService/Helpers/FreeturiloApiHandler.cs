using FreeturiloWebApi.DTO;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NextBikeApiService.Helpers
{
    static class FreeturiloApiHandler
    {
        private static readonly string _serwerPath = @"https://localhost:5001/";

        private const string email = "freeturilo@gmail.com";
        private const string password = "Freeturilo123PW!";

        private const string tokenParameters = "user";
        private const string stationsParameters = "station";

        public static string GetToken(CancellationToken stoppingToken)
        {
            var client = new RestClient(_serwerPath + tokenParameters)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{""email"": """ + email + @""", ""password"": """ + password + @""" }";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string token = response.Content[1..^1];
            return token;
        }

        public static IEnumerable<StationDTO> GetFreeturiloStations(CancellationToken stoppingToken)
        {
            var client = new RestClient(_serwerPath + stationsParameters)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            var jsonContent = response.Content;

            var stations = JsonSerializer.Deserialize<StationDTO[]>(jsonContent);
            return stations;
        }

    }
}
