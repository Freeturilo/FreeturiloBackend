using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public interface IAppMethods
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        void Start(string serverPath, string token);
        /// <summary>
        /// Returns state of application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        int Status(string serverPath, string token);
        /// <summary>
        /// Returns report threshold of admin
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetReportThreshold(string serverPath, string token);
        /// <summary>
        /// Stops the applciation
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        void Stop(string serverPath, string token);
        /// <summary>
        /// Sets demo state of application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        void Demo(string serverPath, string token);
        /// <summary>
        /// Sets report threshold of admin
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="number"></param>
        void SetReportThreshold(string serverPath, string token, int number);
    }
}