using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Helpers.Graph;
using FreeturiloWebApi.Models;
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
        /// <summary>
        /// Max time of ride with one bike
        /// </summary>
        public override int FreeTime => 20 * 60;
        /// <summary>
        /// Indicates if solver can be used
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 0;
        }
    }
}
