using FreeturiloWebApi.DTO;
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

        public static bool Start(string serverPath, string token)
        {
            var client = new RestClient(serverPath + start)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            return client.Execute(request).StatusCode == System.Net.HttpStatusCode.OK;
        }

        public static bool Stop(string serverPath, string token)
        {
            var client = new RestClient(serverPath + stop)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            return client.Execute(request).StatusCode == System.Net.HttpStatusCode.OK;
        }

        public static bool Demo(string serverPath, string token)
        {
            var client = new RestClient(serverPath + demo)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var code = client.Execute(request).StatusCode;
            return code == System.Net.HttpStatusCode.OK;

        }
    }
}
