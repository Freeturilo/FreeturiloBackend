using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public static class AppMethods
    {
        private const string stop = "app/stop";
        private const string start = "app/start";
        private const string demo = "app/demo";

        public static void Start(string serverPath, string token)
        {
            var client = new RestClient(serverPath + start)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();
        }

        public static void Stop(string serverPath, string token)
        {
            var client = new RestClient(serverPath + stop)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();
        }

        public static void Demo(string serverPath, string token)
        {
            var client = new RestClient(serverPath + demo)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();
        }
        public static void SetReportTrashold(string serverPath, string token, int number)
        {
            var client = new RestClient(serverPath + $"app/notify/{number}")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();
        }
    }
}
