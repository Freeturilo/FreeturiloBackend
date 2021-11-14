using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using FreeturiloWebApi.Models;
using RestSharp;
using FreeturiloWebApi.Exceptions;
using System.Text.Json;
using FreeturiloWebApi.DTO.GoogleDTO;

namespace FreeturiloWebApi.Helpers
{
    static public class GoogleMapsAPIHandler
    {
        private const string serverPath = @"https://maps.googleapis.com/maps/api/directions/json";
        private const string mode = "mode=bicycling";
        private const string units = "units=metric";
        private const string key = "AIzaSyDDYfuFzFNrLnGW-JiyHhd-Aq7-SX1n_Hs";
        private static double CalculateCost(int seconds)
        {
            int minutes = (int)Math.Ceiling(seconds / 60.0);
            if (minutes < 20) return 0;
            int[] costOfHour = new int[] { 1, 3, 5, 7 };
            
            int cost = 0;
            int hour = 0;
            while(minutes > 0)
            {
                cost += costOfHour[hour];
                minutes -= 60;
                hour++;
            }

            return cost;
        }

        public static Models.Route MakeRoute(Station station1, Station station2)
        {
            string orgn = $"origin={station1.Lat.Value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},{station1.Lon.Value.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)}";
            string dest = $"destination={station2.Lat.Value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},{station2.Lon.Value.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)}";

            string path = $"{serverPath}?{orgn}&{dest}&{mode}&{units}&key={key}";

            var client = new RestClient(path)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var res = client.Execute(request);
            if (!res.IsSuccessful) throw new Exception503();

            var direction = JsonSerializer.Deserialize<DirectionDTO>(res.Content);
            var time = direction.Routes[0].Legs[0].Duration.Value;

            return new Models.Route()
            {
                StartId = station1.Id,
                StopId = station2.Id,
                Cost = CalculateCost(time),
                Time = time,
                RouteJSON = res.Content,
            };
        }
    }
}
