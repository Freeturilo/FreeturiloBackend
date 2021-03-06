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
    public abstract class AuthController : ControllerBase
    {
        /// <summary>
        /// Attached administrator
        /// </summary>
        public Administrator Administrator => (Administrator)HttpContext.Items["User"];
    }
}
