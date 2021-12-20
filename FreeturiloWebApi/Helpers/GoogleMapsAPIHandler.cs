using FreeturiloWebApi.DTO;
using FreeturiloWebApi.DTO.GoogleDTO;
using FreeturiloWebApi.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers
{
    public static class GoogleMapsAPIHandler
    {
        private const string token = "AIzaSyDMR-9Yg8xuxBDKCpa85Rw4qT30qV8EVkE";
        private const string serverPath = @"https://maps.googleapis.com/maps/api/directions/json?";
        public static RouteDTO GetRoute(List<LocationDTO> stops, string mode = "bicycling")
        {
            var parameters = $"key={token}&mode={mode}&units=metric&";
            parameters += $"origin={stops[0].Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{stops[0].Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&";
            parameters += $"destination={stops[^1].Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{stops[^1].Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&";

            if(stops.Count > 2)
            {
                var waypoints = "waypoints=";
                for (int i = 1; i < stops.Count - 1; i++)
                {
                    waypoints += $"via:{stops[i].Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{stops[i].Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}|";
                }
                parameters += waypoints[0..^1];
            }

            var client = new RestClient(serverPath + parameters)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);

            var direction = JsonSerializer.Deserialize<DirectionDTO>(response.Content);
            var cost = 0.0;

            if(mode == "bicycling")
            {
                if (stops.Count == 2)
                {
                    cost = CalculateCost(direction.Routes[0].Legs[0].Duration.Value);
                }
                else
                {
                    int i = 2;
                    StationDTO station = stops[1] as StationDTO;
                    int step = direction.Routes[0].Legs[0].ViaWaypoint[0].StepIndex;
                    while (station.Id != (stops[^2] as StationDTO).Id)
                    {
                        while (!(stops[i] is StationDTO))
                        {
                            i++;
                        }
                        int nextStep = direction.Routes[0].Legs[0].ViaWaypoint[i - 1].StepIndex;
                        var time = 0;
                        for (int j = step; j < nextStep; j++)
                        {
                            time += direction.Routes[0].Legs[0].Steps[j].Duration.Value;
                        }
                        cost += CalculateCost(time);

                        station = stops[i] as StationDTO;
                        step = nextStep;
                        i++;
                    }
                }
            }

            var route = new RouteDTO()
            {
                Waypoints = stops.ToArray(),
                DirectionsRoute = direction.Routes[0],
                Time = direction.Routes[0].Legs[0].Duration.Value,
                Cost = cost,
            };
            return route;
        }

        private static double CalculateCost(int time)
        {
            var cost = 0.0;
            int minutes = (int)Math.Ceiling(time / 60.0);
            if (minutes <= 20) return cost;

            double[] hourCost = new double[] { 1, 3, 5, 7 };
            int i = 0;
            while(minutes > 0)
            {
                var costOfHour = hourCost[Math.Min(i, hourCost.Length - 1)];
                cost += costOfHour;
                i++;
                minutes -= 60;
            }

            return cost;
        }
    }
}
