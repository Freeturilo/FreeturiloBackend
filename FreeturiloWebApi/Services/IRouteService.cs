using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public interface IRouteService
    {
        /// <summary>
        /// Return route based on route parameters
        /// </summary>
        /// <param name="routeParameters">Route parameters</param>
        /// <returns></returns>
        FragmentRouteDTO[] GetRoute(RouteParametersDTO routeParameters);
    }
}
