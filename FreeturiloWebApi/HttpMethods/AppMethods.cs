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
    public class AppMethods: IAppMethods
    {
        private const string stop = "app/state/2";
        private const string start = "app/state/0";
        private const string demo = "app/state/1";
        private const string status = "app/state";

        /// <summary>
        /// Starts the apllication
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        public void Start(string serverPath, string token)
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

        /// <summary>
        /// Returns state of application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public int Status(string serverPath, string token)
        {
            var client = new RestClient(serverPath + status)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();

            var jsonContent = res.Content;
            var result = JsonSerializer.Deserialize<int>(jsonContent);
            return result;
        }

        /// <summary>
        /// Stops the application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        public void Stop(string serverPath, string token)
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

        /// <summary>
        /// Sets demo state of application
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        public void Demo(string serverPath, string token)
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

        /// <summary>
        /// Sets report threshold of admin
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="number"></param>
        public void SetReportThreshold(string serverPath, string token, int number)
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

        /// <summary>
        /// Return report threshold of admin
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetReportThreshold(string serverPath, string token)
        {
            var client = new RestClient(serverPath + $"app/notify")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) throw new Exception401();
            var jsonContent = res.Content;
            var result = JsonSerializer.Deserialize<int>(jsonContent);
            return result;
        }
    }
}
