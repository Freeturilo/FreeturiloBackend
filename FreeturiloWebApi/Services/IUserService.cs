using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="auth">Email and password</param>
        /// <returns>Return JWT token</returns>
        string Authenticate(AuthDTO auth);
    }
}
