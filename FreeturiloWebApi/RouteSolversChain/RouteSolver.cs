using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers.Graph;
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
        protected abstract BidirectionalGraph<LocationDTO, GraphEdge> CreateLocationsGraph();
        protected abstract double EdgeWeight(GraphEdge e);
        protected List<LocationDTO> UseSolver(List<LocationDTO> stops)
        {
            //TODO poprawić to
            var graph = CreateLocationsGraph();
            var finalStops = new List<LocationDTO>();

            for (int i = 0; i < stops.Count - 1; i++)
            {
                var algorithm = new DijkstraShortestPathAlgorithm<LocationDTO, GraphEdge>(graph, EdgeWeight);
                var observer = new VertexPredecessorPathRecorderObserver<LocationDTO, GraphEdge>();

                using (observer.Attach(algorithm))
                {
                    algorithm.Compute(stops[0]);
                    var path = observer.AllPaths().Where(path =>
                    {
                        var last = path.LastOrDefault();
                        if (last == null) throw new Exception404();

                        return last.Target == stops[i + 1];
                    }).FirstOrDefault();

                    if (path == null) throw new Exception404();
                    foreach (var edge in path)
                    {
                        finalStops.Add(edge.Target);
                    }
                }

            }

            return finalStops;
        }
        protected abstract bool SelectSolver(RouteParametersDTO parameters);
        public RouteSolver(IRouteSolver next)
        {
            Next = next;
        }
        public List<LocationDTO> Solve(RouteParametersDTO parametersDTO, List<LocationDTO> stops)
        {
            if (SelectSolver(parametersDTO)) return UseSolver(stops);
            if (Next != null) return Next.Solve(parametersDTO, stops);
            return null;
        }
    }
}
