using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
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

        protected override (List<LocationDTO> stops, string mode) UseSolver(List<LocationDTO> stops, FreeturiloContext context, IMapper mapper)
        {
            throw new NotImplementedException();
        }
    }
}
