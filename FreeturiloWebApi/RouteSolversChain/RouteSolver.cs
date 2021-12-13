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
        public IRouteSolver Next { get; }
        protected abstract double EdgeWeight(GraphEdge e);
        protected abstract bool SelectSolver(RouteParametersDTO parameters);
        private float Heuristic(FastNode source, FastNode target)
        {
            float R = 6371000.0F;
            var fi1 = source.Location.Latitude * Math.PI / 180.0;
            var fi2 = target.Location.Latitude * Math.PI / 180.0;
            var delta_fi = fi2 - fi1;
            var delta_hi = (source.Location.Longitude - target.Location.Longitude) * Math.PI / 180.0;

            var a = Math.Sin(delta_fi / 2) * Math.Sin(delta_fi / 2) + Math.Cos(fi1) * Math.Cos(fi2) * Math.Sin(delta_hi / 2) * Math.Sin(delta_hi / 2);
            float c = 2 * (float)Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;

            return d;
        }
        private (List<LocationDTO>, int, double) AStar(StationDTO[] mappedStations, LocationDTO start, LocationDTO stop, FreeturiloContext context)
        {          
            var nodes = new List<FastNode>();
            var startNode = new FastNode(start);
            foreach (var station in mappedStations) 
                if(station != start)
                    nodes.Add(new FastNode(station));

            var distances = new Dictionary<FastNode, (float distance, List<LocationDTO> path)>();
            foreach (var node in nodes) distances.Add(node, (float.PositiveInfinity, new()));

            nodes.Add(startNode);

            
            var queue = new FastPriorityQueue<FastNode>(2 + mappedStations.Length);
            foreach (var node in nodes) queue.Enqueue(node, distances[node].distance);

            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                if(u.Location == stop)
                {
                    break; 
                }

                foreach(var station in mappedStations)
                {
                    if (station == u.Location) continue;
                    if(distances[u])
                }
            }
        }

        protected virtual (List<LocationDTO> stops, string mode) UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var stations = context.Stations
                .Where(s => s.State != 2 && s.AvailableBikes > 0)
                .ToArray();
            
            var mappedStations = mapper.Map<StationDTO[]>(stations);


            var finalStops = new List<LocationDTO>() { stops[0] };
            var time = 0;
            var cost = 0.0;

            var finalStop = stops[^1];

            stops[0] = GraphMethodsProvider.GetClosestStations(mappedStations, stops[0], 1)[0];
            stops[^1] = GraphMethodsProvider.GetClosestStations(mappedStations, stops[^1], 1)[0];


            for (int i = 0; i< stops.Count - 1; i++)
            {
                (var directLocations, var directTime, var directCost) = AStar(mappedStations, stops[i], stops[i + 1], context);
                finalStops.AddRange(directLocations);
                time += directTime;
                cost += directCost;
            }

            finalStops.Add(stops[^1]);
            finalStops.Add(finalStop);

            var walkingRoute = GoogleMapsAPIHandler.GetRoute(stops, "walking");
            
            var walkingEdge = new GraphEdge(stops[0], stops[^1], walkingRoute.Cost, walkingRoute.Time);
            var cyclingEdge = new GraphEdge(stops[0], stops[^1], cost, time);

            if (EdgeWeight(walkingEdge) < EdgeWeight(cyclingEdge))
                return (stops, "walkikng");

            return (finalStops, "bicycling");

        }
        public RouteSolver(IRouteSolver next)
        {
            Next = next;
        }
        public (List<LocationDTO> stops, string mode) Solve(RouteParametersDTO parametersDTO, List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            if (SelectSolver(parametersDTO)) return UseSolver(stops, context, mapper);
            if (Next != null) return Next.Solve(parametersDTO, stops, context, mapper);

            throw new Exception400();
        }
    }
}
