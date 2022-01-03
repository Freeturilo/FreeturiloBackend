using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Middlewares
{
    public class JwtMiddleware : IMiddleware
    {
        private readonly FreeturiloContext context;
        private readonly IOptions<AppSettings> appSettings;

        public JwtMiddleware(FreeturiloContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings;
        }
        /// <summary>
        /// Verifies token from request
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["api-key"].FirstOrDefault()?.Split(" ").Last();

            if(token != null)
            {
                AttachUserToContext(context, token);
            }

            await next(context);
        }
        /// <summary>
        /// Attaches user to context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Value.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(30),
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                context.Items["User"] = this.context.Administrators.Where(a => a.Id == userId).FirstOrDefault();
            }
            catch
            {

            }
        }
    }
}
