using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers.Graph;
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
        protected override BidirectionalGraph<LocationDTO, GraphEdge> CreateLocationsGraph()
        {
            throw new NotImplementedException();
        }

        protected override double EdgeWeight(GraphEdge e)
        {
            return e.Cost + e.Time / 20;
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 2;
        }
    }
}
