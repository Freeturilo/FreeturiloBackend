using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public interface IAppService
    {
        void Demo();
        void Start();
        void Stop();
        void SetReportTrashold(int id, int number);
    }
}
