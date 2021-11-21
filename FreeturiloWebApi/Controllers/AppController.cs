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
        [HttpGet]
        public ActionResult<int> Status()
        {
            var status = _service.Status();
            return Ok(status);
        }

        [Auth]
        [HttpPost("start")]
        public ActionResult Start()
        {
            _service.Start();
            return Ok();
        }

        [Auth]
        [HttpPost("stop")]
        public ActionResult Stop()
        {
            _service.Stop();
            return Ok();
        }

        [Auth]
        [HttpPost("demo")]
        public ActionResult Demo()
        {
            _service.Demo();
            return Ok();
        }

        [Auth]
        [HttpPost("notify/{number}")]
        public ActionResult SetReportTrashold([FromRoute] int number)
        {
            _service.SetReportTrashold(Administrator.Id, number);
            return Ok();
        }

    }
}
