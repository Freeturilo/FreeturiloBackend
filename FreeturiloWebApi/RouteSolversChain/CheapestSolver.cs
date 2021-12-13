﻿using AutoMapper;
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
    class CheapestRouteSolver: RouteSolver
    {
        public CheapestRouteSolver(IRouteSolver next) : base(next) { }

        protected override float EdgeWeight(int time, double cost)
        {
            return (float)cost;
        }

        protected override bool SelectSolver(RouteParametersDTO parameters)
        {
            return parameters.Criterion == 0;
        }
    }
}
