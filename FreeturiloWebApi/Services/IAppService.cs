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
        /// <summary>
        /// Sets specific status of application
        /// </summary>
        /// <param name="state"></param>
        void SetStatus(int state);
        /// <summary>
        /// Set report treshold for admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        void SetReportTrashold(int id, int number);
        /// <summary>
        /// Returns current status of application
        /// </summary>
        /// <returns></returns>
        int Status();
    }
}
