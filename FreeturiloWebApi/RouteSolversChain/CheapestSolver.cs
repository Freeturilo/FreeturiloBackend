using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers.Graph;
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
        protected override BidirectionalGraph<LocationDTO, GraphEdge> CreateLocationsGraph()
        {
            throw new NotImplementedException();
        }

        protected override double EdgeWeight(GraphEdge e)
        {
            return e.Cost;
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 0;
        }
    }
}
