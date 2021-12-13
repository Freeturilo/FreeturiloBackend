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
        protected override float EdgeWeight(int time, double cost)
        {
            return (float)(time + cost * 20);
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 2;
        }
    }
}
