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

        [HttpPost]
        [AppState]
        public ActionResult<RouteDTO[]> GetAllStations([FromBody] RouteParametersDTO routeParameters)
        {
            var route = _service.GetRoute(routeParameters);
            return Ok(route);
        }
    }
}
