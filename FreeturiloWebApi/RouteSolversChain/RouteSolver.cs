using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
using Priority_Queue;
using QuikGraph;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.RouteSolversChain
{
    public abstract class RouteSolver : IRouteSolver
    {
        /// <summary>
        /// Next element in chain
        /// </summary>
        public IRouteSolver Next { get; }
        /// <summary>
        /// Max time of ride with one bike
        /// </summary>
        public abstract int FreeTime { get; }
        /// <summary>
        /// Max cost of ride with one bike
        /// </summary>
        public abstract int FreeCost { get; }
        public RouteSolver(IRouteSolver next)
        {
            Next = next;
        }

        /// <summary>
        /// Indicates if solver can be selected
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected abstract bool SelectSolver(RouteParametersDTO parameters);
        /// <summary>
        /// Return stops if solver used
        /// </summary>
        /// <param name="stops"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        protected virtual List<LocationDTO> UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var stations = context.Stations.Where(s => s.AvailableBikes > 0 && s.State != 2).ToArray();
            var mappedStations = mapper.Map<StationDTO[]>(stations);
            var start = stops[0];
            var closestStation = GraphMethodsProvider.GetClosestStations(mappedStations, start, 1)[0];

            var finalStops = new List<LocationDTO>() { stops[0] };

            var i = 0;
            LocationDTO currentStop = closestStation;
            StationDTO lastStation = closestStation;
            var maxTime = FreeTime;
            while (currentStop != stops[^1])
            {
                (maxTime, lastStation) = FindPartOfPath(finalStops, maxTime, lastStation, stops[i + 1], mappedStations, context, mapper);
                i++;
                currentStop = stops[i];
            }

            return finalStops;
        }
        /// <summary>
        /// Returns parf of path based on last station and max time
        /// </summary>
        /// <param name="finalStops"></param>
        /// <param name="maxTime"></param>
        /// <param name="lastStation"></param>
        /// <param name="stop"></param>
        /// <param name="stations"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        private (int maxTime, StationDTO lastStation) FindPartOfPath(List<LocationDTO> finalStops, int maxTime, StationDTO lastStation, LocationDTO stop, StationDTO[] stations, FreeturiloContext context, IMapper mapper)
        {
            var closestStation = GraphMethodsProvider.GetClosestStations(stations, stop, 1)[0];
            if(closestStation == lastStation)
            {
                finalStops.Add(stop);
                var toStopPath = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { closestStation, stop });

                return (FreeTime - 2 * toStopPath.Time, closestStation);
            }

            var routesToStop = context.Routes.Where(r => r.StopId == closestStation.Id).ToArray();

            StationDTO bestStation = null;
            StationDTO betterStation = null;
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

                    if (timeToStation <= maxTime)
                    {
                        var newTimeToStop = 0;
                        if (station != closestStation)
                            newTimeToStop = routesToStop.Where(r => r.StartId == station.Id).FirstOrDefault().Time;
                        if (newTimeToStop < bestTime)
                        {
                            bestTime = newTimeToStop;
                            bestStation = station;
                        }
                    }
                }
            }

            if (bestStation != null)
            {
                finalStops.Add(bestStation);
                lastStation = bestStation;
            }
            else
            {
                finalStops.Add(lastStation);
            }

            bool firstTimeFlag = true;
            while (lastStation != closestStation)
            {
                if (firstTimeFlag && bestStation == null && routesFromLastStation != null)
                {
                    firstTimeFlag = false;
                }
                else
                {
                    routesFromLastStation = context.Routes.Where(r => r.StartId == lastStation.Id).ToArray();
                }

                bestStation = null;
                betterStation = null;
                bestTime = int.MaxValue;
                bestCost = double.PositiveInfinity;

                foreach (var station in stations)
                {
                    if (station == lastStation) continue;
                    double costToStation = routesFromLastStation.Where(r => r.StopId == station.Id).FirstOrDefault().Cost;
                    if (costToStation <= bestCost || costToStation <= FreeCost)
                    {
                        int newTimeToStop = 0;
                        if (station != closestStation)
                            newTimeToStop = routesToStop.Where(r => r.StartId == station.Id).FirstOrDefault().Time;
                        if ((FreeCost < bestCost && costToStation < bestCost) || (FreeCost >= bestCost && costToStation <= FreeCost && newTimeToStop < bestTime))
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

            return (FreeTime - 2 * lastPartOfPath.Time, closestStation);
        }
        /// <summary>
        /// Return stops of delegates call to another solver in chain
        /// </summary>
        /// <param name="parametersDTO"></param>
        /// <param name="stops"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public List<LocationDTO> Solve(RouteParametersDTO parametersDTO, List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            if (SelectSolver(parametersDTO)) return UseSolver(stops, context, mapper);
            if (Next != null) return Next.Solve(parametersDTO, stops, context, mapper);

            throw new Exception400();
        }
    }
}
