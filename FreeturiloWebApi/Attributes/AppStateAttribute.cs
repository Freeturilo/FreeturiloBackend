using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FreeturiloWebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (Administrator)context.HttpContext.Items["User"];
            if(user == null)
            {
                throw new Exception401("Brak dostępu");
            }
        }
    }
}
