using FreeturiloWebApi.Attributes;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Controllers
{
    [Controller]
    [Route("/app")]
    public class AppController : AuthController
    {
        private readonly IAppService _service;
        public AppController(IAppService service)
        {
            _service = service;
        }
        [Auth]
        [HttpGet("state")]
        public ActionResult<int> Status()
        {
            var status = _service.Status();
            return Ok(status);
        }
        [Auth]
        [HttpPost("state/{state}")]
        public ActionResult SetState([FromRoute] int state)
        {
            _service.SetStatus(state);
            return Ok();
        }

        [Auth]
        [HttpPost("notify/{number}")]
        public ActionResult SetReportTrashold([FromRoute] int number)
        {
            _service.SetReportTrashold(Administrator.Id, number);
            return Ok();
        }
        
        [Auth]
        [HttpGet("notify")]
        public ActionResult<int> GetReportTrashold()
        {
            return Ok(Administrator.NotifyThreshold);
        }
    }
}
