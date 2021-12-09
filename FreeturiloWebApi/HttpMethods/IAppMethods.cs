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
        void Start(string serverPath, string token);
        int Status(string serverPath, string token);
        int GetReportTrashold(string serverPath, string token);
        void Stop(string serverPath, string token);
        void Demo(string serverPath, string token);
        void SetReportTrashold(string serverPath, string token, int number);
    }
}
