using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
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
        protected abstract (List<LocationDTO> stops, string mode) UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper);
        protected abstract bool SelectSolver(RouteParametersDTO parameters);
        public RouteSolver(IRouteSolver next)
        {
            Next = next;
        }
        public (List<LocationDTO> stops, string mode) Solve(RouteParametersDTO parametersDTO, List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            if (SelectSolver(parametersDTO)) return UseSolver(stops, context, mapper);
            if (Next != null) return Next.Solve(parametersDTO, stops, context, mapper);
            return (null, null);
        }
    }
}
