using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public class RouteService : IRouteService
    {
        private readonly FreeturiloContext _context;
        private readonly IMapper _mapper;

        public RouteService(FreeturiloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private const double minLon = 20.851666;
        private const double maxLon = 21.271111;
        private const double minLat = 52.097777;
        private const double maxLat = 52.368055;

        private static bool IsInWarsaw(LocationDTO location)
        {
            if (location.Latitude >= minLat && location.Latitude <= maxLat)
                if (location.Longitude >= minLon && location.Longitude <= maxLon)
                    return true;

            return false;
        }
        public RouteDTO GetRoute(RouteParametersDTO routeParameters)
        {
            if (routeParameters == null) throw new Exception400();
            if (routeParameters.Start == null || routeParameters.End == null) throw new Exception400();
            if (routeParameters.Criterion < 0 || routeParameters.Criterion > 2) throw new Exception400();

            var stops = new List<LocationDTO> { routeParameters.Start };
            if (routeParameters.Stops != null)
                stops.AddRange(routeParameters.Stops);
            stops.Add(routeParameters.End);

            if (stops.Any(stop => !IsInWarsaw(stop))) throw new Exception404();


            RouteDTO route = GoogleMapsAPIHandler.GetRoute(stops, "walking");

            if (routeParameters.Criterion == 0)
            {
                var r = GetCheapestRoute(stops);
                if (r.Cost <= route.Cost)
                    route = r;
            }
            else if (routeParameters.Criterion == 1)
            {
                var r = GetFastestRoute(stops);
                if (r.Time <= route.DirectionsRoute.Routes[0].Legs[0].Duration.Value)
                    route = r;
            }
            else if(routeParameters.Criterion == 2)
            {
                var r = GetOptimalRoute(stops);
                if (true /*TODO optimal condition*/)
                    route = r;
            }

            return route;
        }
        private RouteDTO GetOptimalRoute(List<LocationDTO> stops)
        {
            throw new NotImplementedException();
        }
        private RouteDTO GetCheapestRoute(List<LocationDTO> stops)
        {
            StationDTO startStation = GetClosestStation(stops[0]);
            StationDTO endStation = GetClosestStation(stops[^1]);

            var routeStops = new List<LocationDTO>
            {
                stops[0],
                startStation
            };

            var currentStation = startStation;
            for(int i = 1; i < stops.Count - 1; i++)
            {
                var stop = stops[i];
                var closestStation = GetClosestStation(stop);
                var middleStations = GetMiddleStations(currentStation, closestStation);

                routeStops.AddRange(middleStations);
                routeStops.Add(stop);
                routeStops.Add(closestStation);

                currentStation = closestStation;
            }

            routeStops.Add(endStation);

            RouteDTO route = GoogleMapsAPIHandler.GetRoute(routeStops);
            return route;
        }
        private List<LocationDTO> GetMiddleStations(StationDTO startStation, StationDTO endStation)
        {
            var middleStations = new List<LocationDTO>();
            var station = startStation;
            while(station.Id != endStation.Id)
            {
                double minCost = double.PositiveInfinity;
                Station closest = null;
                var toEndRoute = _context.Routes.Where(r => r.StartId == station.Id && r.StopId == endStation.Id).FirstOrDefault();
                double actualTime = toEndRoute == null ? double.PositiveInfinity : toEndRoute.Time;

                foreach(var nextRoute in _context.Routes.Where(r => r.StartId == station.Id))
                {
                    var nextStation = _context.Stations.Where(s => s.Id == nextRoute.StopId).FirstOrDefault();
                    if (nextStation == null || nextStation.AvailableBikes == 0 || nextStation.State == 2) continue;
                    var nextStationToEndRoute = _context.Routes.Where(r => r.StartId == nextStation.Id && r.StopId == endStation.Id).FirstOrDefault();

                    var nextStationTime = double.PositiveInfinity;
                    if(nextStation.Id == endStation.Id)
                    {
                        nextStationTime = 0;
                    }
                    else if(nextStationToEndRoute != null)
                    {
                        nextStationTime = nextStationToEndRoute.Time;
                    }
                    else
                    {
                        continue;
                    }

                    if (nextStationTime < actualTime)
                    {
                        var routeToNextStation = _context.Routes.Where(r => r.StartId == station.Id && r.StopId == nextStation.Id).FirstOrDefault();
                        if(routeToNextStation != null && routeToNextStation.Cost <= minCost)
                        {
                            minCost = routeToNextStation.Cost;
                            closest = nextStation;
                            actualTime = nextStationToEndRoute.Time;
                        }
                    }
                }

                var closestStation = _mapper.Map<StationDTO>(closest);
                middleStations.Add(closestStation);
                station = closestStation;
            }

            return middleStations;
        }
        private RouteDTO GetFastestRoute(List<LocationDTO> stops)
        {
            LocationDTO startStation = GetClosestStation(stops[0]);
            LocationDTO endStation = GetClosestStation(stops[^1]);

            stops.Insert(1, startStation);
            stops.Insert(stops.Count - 1, endStation);
            
            RouteDTO route = GoogleMapsAPIHandler.GetRoute(stops);
            return route;
        }
        private StationDTO GetClosestStation(LocationDTO locationDTO)
        {
            Station closestStation = null;
            double closestDistance = double.PositiveInfinity;
            foreach(var station in _context.Stations)
            {
                if (station.AvailableBikes == 0 || station.State == 2) continue;
                double distance = CalculateDistance(locationDTO, station);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestStation = station;
                }
            }
            var closestStationDTO = _mapper.Map<StationDTO>(closestStation);
            return closestStationDTO;
        }
        private static double CalculateDistance(LocationDTO l1, Station l2)
        {
            double c = (l1.Latitude - l2.Lat) * (l1.Latitude - l2.Lat) + (l1.Longitude - l2.Lon) * (l1.Longitude - l2.Lon);
            return c;
        }
    }
}
