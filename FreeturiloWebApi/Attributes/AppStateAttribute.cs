using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace FreeturiloWebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppStateAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var dbContext = context.HttpContext
                .RequestServices
                .GetService(typeof(FreeturiloContext)) as FreeturiloContext;

            var appState = dbContext.State.FirstOrDefault();
            if (appState == null || appState.Value == 2)
            {
                throw new Exception503();
            }
        }
    }
}
