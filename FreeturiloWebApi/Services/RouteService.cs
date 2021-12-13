using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.RouteSolversChain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuikGraph;
using QuikGraph.Algorithms.Observers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public class RouteService : IRouteService
    {
        private readonly FreeturiloContext _context;
        private readonly IMapper _mapper;

        public RouteService(FreeturiloContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private const double minLon = 20.851666;
        private const double maxLon = 21.271111;
        private const double minLat = 52.097777;
        private const double maxLat = 52.368055;

        private static bool IsInWarsaw(LocationDTO location)
        {
            if (location.Latitude >= minLat && location.Latitude <= maxLat)
                if (location.Longitude >= minLon && location.Longitude <= maxLon)
                    return true;

            return false;
        }

        public RouteDTO GetRoute(RouteParametersDTO routeParameters)
        {
            if (routeParameters == null) throw new Exception400();
            if (routeParameters.Start == null || routeParameters.End == null) throw new Exception400();

            var stops = new List<LocationDTO> { routeParameters.Start };
            if (routeParameters.Stops != null)
                stops.AddRange(routeParameters.Stops);
            stops.Add(routeParameters.End);

            if (stops.Any(stop => !IsInWarsaw(stop))) throw new Exception404();

            IRouteSolver solver = new FastestRouteSolver(null);
            solver = new CheapestRouteSolver(solver);
            solver = new OptimalRouteSolver(solver);

            (var finalStops, var mode) = solver.Solve(routeParameters, stops, _context, _mapper);
            var route =  GoogleMapsAPIHandler.GetRoute(finalStops, mode);
            route.Parameters = routeParameters;
            return route;
        }
    }
}
