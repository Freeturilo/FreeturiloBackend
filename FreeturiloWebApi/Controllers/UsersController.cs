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
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<string> Authenticate([FromBody] AuthDTO auth)
        {
            string token = _service.Authenticate(auth);
            return Ok(token);
        }

    }
}
