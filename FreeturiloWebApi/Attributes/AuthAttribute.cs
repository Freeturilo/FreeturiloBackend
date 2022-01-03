using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FreeturiloWebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Checks if admin in authorized
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (Administrator)context.HttpContext.Items["User"];
            if(user == null)
            {
                throw new Exception401();
            }
        }
    }
}
