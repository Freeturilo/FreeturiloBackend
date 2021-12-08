using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.RouteSolversChain
{
    class FastestRouteSolver: RouteSolver
    {
        public FastestRouteSolver(IRouteSolver next) : base(next) { }

        protected override double EdgeWeight(GraphEdge e)
        {
            return e.Time;
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 1;
        }

        protected override (List<LocationDTO> stops, string mode) UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            if(stops.Count == 2)
            {
                return FindLocationsWithoutStops(stops, context, mapper);
            }
            else
            {
                return FindLocationsWithStops(stops, context, mapper);
            }
            
        }

        private (List<LocationDTO> stops, string mode) FindLocationsWithStops(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var start = stops[0];
            var end = stops[^1];

            var stations = context.Stations.Where(s => s.State != 2 && s.AvailableBikes > 0).ToArray();
            var startClosestStations = GraphMethodsProvider.GetClosestStations(stations, start, 2);
            var endClosestStations = GraphMethodsProvider.GetClosestStations(stations, end, 2);

            var minTime = double.PositiveInfinity;
            Station bestStation = null;

            var finalStops = new List<LocationDTO>() { start };

            foreach (var station1 in startClosestStations)
            {
                var route1 = GoogleMapsAPIHandler.GetRoute(new() { start, mapper.Map<StationDTO>(station1) }, "walking");
                var route2 = GoogleMapsAPIHandler.GetRoute(new() { mapper.Map<StationDTO>(station1), stops[1] }, "cycling");

                var time = route1.Time + route2.Time;
                if (time < minTime)
                {
                    minTime = time;
                    bestStation = station1;
                }
            }

            finalStops.Add(mapper.Map<StationDTO>(bestStation));

            for(int i=1; i<stops.Count-1;i++)
            {
                finalStops.Add(stops[i]);
            }

            minTime = double.PositiveInfinity;
            bestStation = null;

            foreach (var station1 in endClosestStations)
            {
                var route1 = GoogleMapsAPIHandler.GetRoute(new() { stops[^2], mapper.Map<StationDTO>(station1) }, "bicycling");
                var route2 = GoogleMapsAPIHandler.GetRoute(new() { mapper.Map<StationDTO>(station1), end }, "walking");

                var time = route1.Time + route2.Time;
                if (time < minTime)
                {
                    minTime = time;
                    bestStation = station1;
                }
            }

            finalStops.Add(mapper.Map<StationDTO>(bestStation));
            finalStops.Add(end);

            var walkingRoute = GoogleMapsAPIHandler.GetRoute(stops, "walking");
            if (walkingRoute.Time < minTime)
                return (new() { start, end }, "walkikng");

            return (finalStops, "bicycling");
        }

        private (List<LocationDTO> stops, string mode) FindLocationsWithoutStops(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var start = stops[0];
            var end = stops[^1];

            var stations = context.Stations.Where(s => s.State != 2 && s.AvailableBikes > 0).ToArray();
            var startClosestStations = GraphMethodsProvider.GetClosestStations(stations, start, 2);
            var endClosestStations = GraphMethodsProvider.GetClosestStations(stations, end, 2);

            var minTime = double.PositiveInfinity;
            (Station s1, Station s2) closestStations = (null, null);

            foreach (var station1 in startClosestStations)
                foreach(var station2 in endClosestStations)
                {
                    var route1 = GoogleMapsAPIHandler.GetRoute(new() { start, mapper.Map<StationDTO>(station1) }, "walking");
                    var route2 = GoogleMapsAPIHandler.GetRoute(new() { mapper.Map<StationDTO>(station1), mapper.Map<StationDTO>(station2) }, "bicycling");
                    var route3 = GoogleMapsAPIHandler.GetRoute(new() { mapper.Map<StationDTO>(station2), end }, "walking");

                    var time = route1.Time + route2.Time + route3.Time;
                    if(time < minTime)
                    {
                        minTime = time;
                        closestStations = (station1, station2);
                    }
                }

            var walkingRoute = GoogleMapsAPIHandler.GetRoute(new() { start, end }, "walking");
            if (walkingRoute.Time < minTime)
                return (new() { start, end }, "walking");

            if (closestStations.s1 == closestStations.s2)
                return (new() { start, mapper.Map<StationDTO>(closestStations.s2), end }, "bicycling");
            else
                return (new() { start, mapper.Map<StationDTO>(closestStations.s1), mapper.Map<StationDTO>(closestStations.s2), end }, "bicycling");
        }
    }
}
