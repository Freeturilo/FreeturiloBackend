using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeturiloWebApi.HttpMethods
{
    public class RouteMethods: IRouteMethods
    {
        const string nmspace = "route";
        public RouteDTO GetRoute(string serverPath, RouteParametersDTO parameters)
        {
            var client = new RestClient(serverPath + nmspace)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            if (parameters != null)
            {
                string body = JsonSerializer.Serialize(parameters);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.BadRequest) throw new Exception400();
            else if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
            else if (response.StatusCode == HttpStatusCode.ServiceUnavailable) throw new Exception503();

            var route = JsonSerializer.Deserialize<RouteDTO>(response.Content);
            return route;
        }
    }
}
