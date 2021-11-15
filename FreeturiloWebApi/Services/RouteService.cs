using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        public RouteService(FreeturiloContext context)
        {
            _context = context;
        }

        public RouteDTO GetRoute(RouteParametersDTO routeParameters)
        {
            throw new NotImplementedException();
        }
    }
}
