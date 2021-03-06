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
        /// <summary>
        /// Max time of ride with one bike
        /// </summary>
        public override int FreeTime => 60 * 60;
        /// <summary>
        /// Max cost of ride with one bike
        /// </summary>
        public override int FreeCost => 1;
        /// <summary>
        /// Indicates if solver can be used
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 2;
        }
    }
}
