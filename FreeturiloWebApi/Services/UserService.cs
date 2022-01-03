using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly FreeturiloContext _context;
        private readonly IOptions<AppSettings> appSettings;

        public UserService(FreeturiloContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            this.appSettings = appSettings;
        }
        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="auth">Email and password</param>
        /// <returns>Return JWT token</returns>
        public string Authenticate(AuthDTO auth)
        {
            if(auth == null || auth.Email == null || auth.Password == null)
            {
                throw new Exception401();
            }
            var hash = PasswordHasher.Hash(auth.Password);
            var user = _context.Administrators.Where(a => a.Email == auth.Email && a.PasswordHash == hash).FirstOrDefault();
            if (user == null) throw new Exception401();

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(Administrator user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
