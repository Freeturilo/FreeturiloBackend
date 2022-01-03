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
        public const int freeTime = 20 * 60;
        /// <summary>
        /// Returns edge weights besad on time and cost
        /// </summary>
        /// <param name="time"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        protected virtual float EdgeWeight(int time, double cost) { return 0; }
        /// <summary>
        /// Indicates if solver can be selected
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected abstract bool SelectSolver(RouteParametersDTO parameters);
        /// <summary>
        /// A* algorithm to establish stops of route
        /// </summary>
        /// <param name="mappedStations"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<LocationDTO> AStar(StationDTO[] mappedStations, StationDTO start, StationDTO stop, FreeturiloContext context)
        {          
            var nodes = new List<FastNode>();
            var startNode = new FastNode(start);
            var endNode = new FastNode(stop);

            foreach (var station in mappedStations) 
                if(station != start && station != stop)
                    nodes.Add(new FastNode(station));
            nodes.Add(endNode);

            var distances = new Dictionary<FastNode, (float distance, List<LocationDTO> path)>();
            foreach (var node in nodes) distances.Add(node, (float.PositiveInfinity, new()));

            distances.Add(startNode, (0, new()));

            var queue = new FastPriorityQueue<FastNode>(2 + mappedStations.Length);
            foreach (var node in nodes) queue.Enqueue(node, distances[node].distance);
            queue.Enqueue(startNode, 0);

            nodes.Add(startNode);

            while (queue.Count > 0)
            {
                var u = queue.Dequeue();
                if(u.Location == stop)
                {
                    return distances[u].path;
                }

                var edges = context.Routes.Where(r => r.StartId == u.Location.Id).ToArray();
                foreach (var node in nodes)
                {
                    if (node == u) continue;
                    var edge = edges.Where(e => e.StopId == node.Location.Id).FirstOrDefault();
                    var newDistance = distances[u].distance + EdgeWeight(edge.Time, edge.Cost);
                    if (distances[node].distance > newDistance)
                    {
                        var newPath = new List<LocationDTO>(distances[node].path);
                        newPath.Add(u.Location);
                        distances[node] = (newDistance, newPath);
                        queue.UpdatePriority(node, newDistance);
                    }
                }
            }

            throw new Exception404();
        }
        /// <summary>
        /// Methed to find stops when solver is selected
        /// </summary>
        /// <param name="stops"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        protected virtual List<LocationDTO> UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var stations = context.Stations
                .Where(s => s.State != 2 && s.AvailableBikes > 0)
                .ToArray();
            
            var mappedStations = mapper.Map<StationDTO[]>(stations);


            var finalStops = new List<LocationDTO>();

            var closestStations = new StationDTO[stops.Count];
            for(int i =0; i< stops.Count;i++)
            {
                closestStations[i] = GraphMethodsProvider.GetClosestStations(mappedStations, stops[i], 1)[0];
            }

            for (int i = 0; i < stops.Count - 1; i++)
            {
                var routes = context.Routes.Where(r => r.StartId == closestStations[i].Id).ToArray();
                var directRoute = routes.Where(r => r.StopId == closestStations[i + 1].Id).FirstOrDefault();
                var directLocations = AStar(mappedStations.Where(s => {
                    var route = routes.Where(r => r.StopId == s.Id).FirstOrDefault();
                    return route != null && route.Time < directRoute.Time;
                }).ToArray(), closestStations[i], closestStations[i + 1], context);
                finalStops.Add(stops[i]);
                finalStops.AddRange(directLocations);
                finalStops.Add(closestStations[i + 1]);
            }
            finalStops.Add(stops[^1]);

            return finalStops;
        }
        public RouteSolver(IRouteSolver next)
        {
            Next = next;
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
