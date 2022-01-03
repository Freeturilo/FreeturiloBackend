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
    public class AppService : IAppService
    {
        private readonly FreeturiloContext _context;
        public AppService(FreeturiloContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Sets report treshold of admin
        /// </summary>
        /// <param name="id">Admin id</param>
        /// <param name="number">Treshold value</param>
        public void SetReportTrashold(int id, int number)
        {
            var admin = _context.Administrators.Where(a => a.Id == id).FirstOrDefault();
            admin.NotifyThreshold = number;
            _context.SaveChanges();
        }
        /// <summary>
        /// Sets state of application
        /// </summary>
        /// <param name="state">State to be set</param>
        public void SetStatus(int state)
        {
            if (state < 0 || state > 2) throw new Exception400();

            var actualState = _context.State.FirstOrDefault();
            actualState.Value = state;
            _context.SaveChanges();
        }
        /// <summary>
        /// Returns curretnt state of application
        /// </summary>
        /// <returns></returns>
        public int Status()
        {
            return _context.State.FirstOrDefault().Value;
        }
    }
}
