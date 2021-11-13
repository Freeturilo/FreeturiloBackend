using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FreeturiloWebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppStateAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var appState = AppState.State;
            if(appState != AppStateEnum.Started)
            {
                throw new Exception503();
            }
        }
    }
}
