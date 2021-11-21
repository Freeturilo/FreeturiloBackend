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
        public void Demo()
        {
            AppState.Demo();
        }
        public void SetReportTrashold(int id, int number)
        {
            var admin = _context.Administrators.Where(a => a.Id == id).FirstOrDefault();

            var oldNumber = admin.NotifyThreshold;
            admin.NotifyThreshold = number;
            _context.SaveChanges();

            if(number < oldNumber)
            {
                foreach(var station in _context.Stations)
                {
                    if(station.Reports > number && oldNumber > station.Reports)
                    {
                        GmailAPIHandler.SendEmail(admin, station);
                    }
                }
            }
        }
        public void Start()
        {
            AppState.Start();
        }

        public int Status()
        {
            return (int)AppState.State;
        }

        public void Stop()
        {
            AppState.Stop();
        }
    }
}
