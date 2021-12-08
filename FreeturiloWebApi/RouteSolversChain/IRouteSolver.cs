using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Helpers.Graph;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.RouteSolversChain
{ 
    public interface IRouteSolver
    {
        IRouteSolver Next { get; }
        List<LocationDTO> Solve( RouteParametersDTO parametersDTO, List<LocationDTO> stops);
    }
}
