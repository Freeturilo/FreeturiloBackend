using FreeturiloWebApi.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        /// <summary>
        /// Return status code based on type of exception
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (FreeturiloException e)
            {
                context.Response.StatusCode = e.StatusCode;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}
