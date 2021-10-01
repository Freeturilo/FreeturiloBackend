using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Controllers
{
    [Controller]
    [Route("/home")]
    public class HomeController : Controller
    {
        private readonly FreeturiloContext _context;
        public HomeController(FreeturiloContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Station> GetStations()
        {
            return _context.Stations.AsEnumerable();
        }
    }
}
