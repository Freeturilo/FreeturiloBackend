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
    public interface IRouteSolver
    {
        /// <summary>
        /// Next solver in chain
        /// </summary>
        IRouteSolver Next { get; }
        /// <summary>
        /// Returns list of locations for a route based on parameters
        /// </summary>
        /// <param name="parametersDTO"></param>
        /// <param name="stops"></param>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        List<LocationDTO> Solve( RouteParametersDTO parametersDTO, List<LocationDTO> stops, FreeturiloContext context, IMapper mapper);
    }
}
