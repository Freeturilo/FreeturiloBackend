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
        /// <summary>
        /// Checks if location is in Warsaw
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static bool IsInWarsaw(LocationDTO location)
        {
            if (location.Latitude >= minLat && location.Latitude <= maxLat)
                if (location.Longitude >= minLon && location.Longitude <= maxLon)
                    return true;

            return false;
        }
        /// <summary>
        /// Return route based on route parameters
        /// </summary>
        /// <param name="routeParameters">Route parameters</param>
        /// <returns></returns>
        public FragmentRouteDTO[] GetRoute(RouteParametersDTO routeParameters)
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

            var finalStops = solver.Solve(routeParameters, stops, _context, _mapper);
            var routes = new FragmentRouteDTO[finalStops.Count-1];
            Parallel.For(0, finalStops.Count - 1, i =>
            {
                FragmentRouteDTO route;
                if(i == 0 || i == finalStops.Count -2)
                {
                    route = GoogleMapsAPIHandler.GetRoute(new() { finalStops[i], finalStops[i + 1] }, "walking");
                }
                else
                {
                    route = GoogleMapsAPIHandler.GetRoute(new() { finalStops[i], finalStops[i + 1] });
                }
                route.Parameters = routeParameters;
                routes[i] = route;
            });

            var totalTime = 0;
            for (int i=1; i<routes.Length-1; i++)
            {
                totalTime += routes[i].Time;
                if(finalStops[i+1] is StationDTO)
                {
                    routes[i].Cost = GoogleMapsAPIHandler.CalculateCost(totalTime);
                    totalTime = 0;
                }
                else
                {
                    routes[i].Cost = 0;
                }
            }

            return routes;
        }
    }
}
