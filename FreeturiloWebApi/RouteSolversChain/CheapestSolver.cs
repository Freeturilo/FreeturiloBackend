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
    class CheapestRouteSolver: RouteSolver
    {
        public CheapestRouteSolver(IRouteSolver next) : base(next) { }
        protected override List<LocationDTO> UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var stations = context.Stations.Where(s => s.AvailableBikes > 0 && s.State != 2).ToArray();
            var mappedStations = mapper.Map<StationDTO[]>(stations);
            var start = stops[0];
            var closestStation = GraphMethodsProvider.GetClosestStations(mappedStations, start, 1)[0];

            var finalStops = new List<LocationDTO>() {stops[0]};

            var i = 0;
            LocationDTO currentStop = closestStation;
            StationDTO lastStation = closestStation;
            var maxTime = 0;
            while (currentStop != stops[^1])
            {
                (maxTime, lastStation) = FindPartOfPath(finalStops, maxTime, lastStation, stops[i+1], mappedStations, context, mapper);
                i++;
                currentStop = stops[i];
            }

            return finalStops;
        }
        private (int maxTime, StationDTO lastStation) FindPartOfPath(List<LocationDTO> finalStops, int maxTime, StationDTO lastStation, LocationDTO stop, StationDTO[] stations, FreeturiloContext context, IMapper mapper)
        {
            var closestStation = GraphMethodsProvider.GetClosestStations(stations, stop, 1)[0];
            var routesToStop = context.Routes.Where(r => r.StopId == closestStation.Id).ToArray();

            StationDTO bestStation = null;
            double bestCost = double.PositiveInfinity;
            int bestTime = int.MaxValue;
            Route[] routesFromLastStation = null;

            if (maxTime > 0)
            {
                bestTime = routesToStop.Where(r => r.StartId == lastStation.Id).FirstOrDefault().Time;
                routesFromLastStation = context.Routes.Where(r => r.StartId == lastStation.Id).ToArray();
                foreach (var station in stations)
                {
                    if (station == lastStation) continue;
                    int timeToStation = routesFromLastStation.Where(r => r.StopId == station.Id).FirstOrDefault().Time;

                    if(timeToStation <= maxTime)
                    {
                        var newTimeToStop = 0;
                        if (station != closestStation)
                            newTimeToStop = routesToStop.Where(r => r.StartId == station.Id).FirstOrDefault().Time;
                        if(newTimeToStop < bestTime)
                        {
                            bestTime = newTimeToStop;
                            bestStation = station;
                        }
                    }
                }
            }

            if(bestStation != null)
            {
                finalStops.Add(bestStation);
                lastStation = bestStation;
            }
            else
            {
                finalStops.Add(lastStation);
            }

            bool firstTimeFlag = true;
            while(lastStation != closestStation)
            {
                if(firstTimeFlag && bestStation == null && routesFromLastStation != null)
                {
                    firstTimeFlag = false;
                }
                else
                {
                    routesFromLastStation = context.Routes.Where(r => r.StartId == lastStation.Id).ToArray();
                }

                bestStation = null;
                bestTime = int.MaxValue;
                bestCost = double.PositiveInfinity;

                foreach (var station in stations)
                {
                    if (station == lastStation) continue;
                    double costToStation = routesFromLastStation.Where(r => r.StopId == station.Id).FirstOrDefault().Cost;
                    if (costToStation <= bestCost)
                    {
                        int newTimeToStop = 0;
                        if (station != closestStation)
                            newTimeToStop = routesToStop.Where(r => r.StartId == station.Id).FirstOrDefault().Time;
                        if (costToStation < bestCost || newTimeToStop < bestTime)
                        {
                            bestTime = newTimeToStop;
                            bestCost = costToStation;
                            bestStation = station;
                        }
                    }
                }

                finalStops.Add(bestStation);
                lastStation = bestStation;
            }

            finalStops.Add(stop);
            var lastPartOfPath = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { closestStation, stop });

            return (freeTime - 2 * lastPartOfPath.Time, closestStation);
        }
        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 0;
        }
    }
}
