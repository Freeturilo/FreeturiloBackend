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
        /// Returns current state of apllication
        /// </summary>
        /// <returns>Returns current state of application</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
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
        /// <returns>Returns information if state has been set properly</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Auth]
        [HttpPost("state/{state}")]
        public ActionResult SetState([FromRoute] int state)
        {
            _service.SetStatus(state);
            return Ok();
        }
        /// <summary>
        /// Sets threshold of administrator
        /// </summary>
        /// <param name="number">Threshold to be set</param>
        /// <returns>Returns information if threshold has been set properly</returns> 
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [Auth]
        [HttpPost("notify/{number}")]
        public ActionResult SetReportThreshold([FromRoute] int number)
        {
            _service.SetReportThreshold(Administrator.Id, number);
            return Ok();
        }
        /// <summary>
        /// Returns threshold of administartor
        /// </summary>
        /// <returns>Return threshold of admin</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [Auth]
        [HttpGet("notify")]
        public ActionResult<int> GetReportThreshold()
        {
            return Ok(Administrator.NotifyThreshold);
        }
    }
}
