using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers.Graph
{
    public static class GraphMethodsProvider
    {
        public static double CalculateDistance(LocationDTO l1, Station l2)
        {
            double c = (l1.Latitude - l2.Lat) * (l1.Latitude - l2.Lat) + (l1.Longitude - l2.Lon) * (l1.Longitude - l2.Lon);
            return c;
        }
        public static Station[] GetClosestStations(FreeturiloContext context, LocationDTO stop, int k = 3)
        {
            var stations = context.Stations.Where(s => s.AvailableBikes > 0 && s.State != 2).ToArray();

            for (int i = 0; i < k; i++)
            {
                for (int j = stations.Length - 1; j >= 1 + i; j--)
                {
                    if (CalculateDistance(stop, stations[j]) < CalculateDistance(stop, stations[j - 1]))
                    {
                        var buf = stations[j];
                        stations[j] = stations[j - 1];
                        stations[j - 1] = buf;
                    }
                }
            }

            var closestStations = new List<Station>();
            for (int i = 0; i < Math.Min(k, stations.Length); i++)
            {
                closestStations.Add(stations[i]);
            }

            return closestStations.ToArray();
        }

    }
}
