using FreeturiloWebApi.Attributes;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Controllers
{
    [Controller]
    [Route("/route")]
    public class RouteController : AuthController
    {
        private readonly IRouteService _service;
        public RouteController(IRouteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns a route based on routeParameters
        /// </summary>
        /// <param name="routeParameters">Parameters of search</param>
        /// <returns>Returns route based on parameters</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not found</response>
        /// <response code="503">Service unavailable</response>
        [HttpPost]
        [AppState]
        public ActionResult<FragmentRouteDTO[]> GetAllStations([FromBody] RouteParametersDTO routeParameters)
        {
            var route = _service.GetRoute(routeParameters);
            return Ok(route);
        }
    }
}
