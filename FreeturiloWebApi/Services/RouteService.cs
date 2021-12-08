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
        
        
        private BidirectionalGraph<LocationDTO, Edge> CreateStationsGraph()
        {
            var graph = new BidirectionalGraph<LocationDTO, Edge>();
            var stations = _context.Stations.Where(s => s.AvailableBikes > 0 && s.State != 2).ToArray();
            var mappedStations = _mapper.Map<LocationDTO[]>(stations);

            graph.AddVertexRange(mappedStations);

            foreach(var source in stations)
            {
                foreach(var target in stations)
                {
                    if (target == source) continue;
                   
                    var route = _context.Routes.Where(r => r.StartId == source.Id && r.StopId == target.Id).FirstOrDefault();
                    if (route == null) continue;

                    var mappedSource = _mapper.Map<LocationDTO>(source);
                    var mappedTarget = _mapper.Map<LocationDTO>(target);
                    var edge = new Edge(mappedSource, mappedTarget, route.Cost, route.Time);

                    graph.AddVerticesAndEdge(edge);
                }
            }

            return graph;            
        }
        private void ExtendGraphWithStops(BidirectionalGraph<LocationDTO, Edge>  graph, IEnumerable<LocationDTO> stops)
        {
            graph.AddVertexRange(stops);

            foreach(var stop in stops)
            {
                var stations = GetClosestStations(stop);
                var mappedStation = _mapper.Map<LocationDTO[]>(stations);

                foreach (var station in mappedStation)
                {
                    var toRoute = GoogleMapsAPIHandler.GetRoute(new() { station, stop });
                    var fromRoute = GoogleMapsAPIHandler.GetRoute(new() { stop, station });

                    var toEdge = new Edge(station, stop, toRoute.Cost, toRoute.Time);
                    var fromEdge = new Edge(stop, station, fromRoute.Cost, fromRoute.Time);

                    graph.AddEdge(toEdge);
                    graph.AddEdge(fromEdge);
                }
            }
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

            var finalStops = solver.Solve(routeParameters, stops);
            return GoogleMapsAPIHandler.GetRoute(finalStops);     
        }
    }
}
