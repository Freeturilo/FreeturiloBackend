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
        /// <summary>
        /// Return current state of apllication
        /// </summary>
        /// <returns>Returns current state of application</returns>
        [Auth]
        [HttpGet("state")]
        public ActionResult<int> Status()
        {
            var status = _service.Status();
            return Ok(status);
        }
        /// <summary>
        /// Sets state of applciation
        /// </summary>
        /// <param name="state">State to be set</param>
        /// <returns>Returns information if state has been properly</returns>
        [Auth]
        [HttpPost("state/{state}")]
        public ActionResult SetState([FromRoute] int state)
        {
            _service.SetStatus(state);
            return Ok();
        }
        /// <summary>
        /// Sets admin's treshold
        /// </summary>
        /// <param name="number">Treshold to be set</param>
        /// <returns>Returns information if treshold has been set properly</returns>
        [Auth]
        [HttpPost("notify/{number}")]
        public ActionResult SetReportTrashold([FromRoute] int number)
        {
            _service.SetReportTrashold(Administrator.Id, number);
            return Ok();
        }
        /// <summary>
        /// Returns admin's treshold
        /// </summary>
        /// <returns>Return treshold of admin</returns>
        [Auth]
        [HttpGet("notify")]
        public ActionResult<int> GetReportTrashold()
        {
            return Ok(Administrator.NotifyThreshold);
        }
    }
}
