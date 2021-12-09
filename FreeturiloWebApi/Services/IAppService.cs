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
        void SetStatus(int state);
        void SetReportTrashold(int id, int number);
        int Status();
    }
}
