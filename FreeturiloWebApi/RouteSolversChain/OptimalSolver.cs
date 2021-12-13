using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
using Priority_Queue;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.RouteSolversChain
{
    class OptimalRouteSolver: RouteSolver
    {
        public OptimalRouteSolver(IRouteSolver next) : base(next) { }

        protected override double EdgeWeight(GraphEdge e)
        {
            return e.Cost + e.Time / 20;
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 2;
        }
        private float Heuristic(FastNode v)
        {
            return 0;
        }

        private class FastNode : FastPriorityQueueNode
        {
            LocationDTO Location { get; set; }
            public FastNode(LocationDTO location) => Location = location;
        }

        protected override (List<LocationDTO> stops, string mode) UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            var stations = context.Stations.Where(s => s.State != 2 && s.AvailableBikes > 0).ToArray();
            var mappedStations = mapper.Map<StationDTO[]>(stations);
            var stationNodes = new List<FastNode>();
            foreach (var station in mappedStations) stationNodes.Add(new FastNode(station));

            var stopNodes = new List<FastNode>();
            foreach (var stop in stops) stopNodes.Add(new FastNode(stop));

            var distances = new Dictionary<FastNode, float>();
            foreach (var node in stationNodes) distances.Add(node, float.PositiveInfinity);
            foreach (var node in stopNodes) distances.Add(node, float.PositiveInfinity);
            distances[stopNodes[0]] = 0;

            var queue = new FastPriorityQueue<FastNode>(stops.Count + mappedStations.Length);

            foreach (var node in stationNodes) queue.Enqueue(node, distances[node] + Heuristic(node));
            foreach (var node in stopNodes) queue.Enqueue(node, distances[node] + Heuristic(node));

            while(queue.Count > 0)
            {
                var u = queue.Dequeue();
                
            }

            return (null, null);
        }
    }
}
