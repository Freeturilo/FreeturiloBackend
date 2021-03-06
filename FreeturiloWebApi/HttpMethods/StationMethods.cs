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
    public class StationMethods: IStationMethods
    {
        const string nmspace = "station";
        /// <summary>
        /// Return all stations
        /// </summary>
        /// <param name="serverPath"></param>
        /// <returns></returns>
        public StationDTO[] GetAllStations(string serverPath)
        {
            var client = new RestClient(serverPath + nmspace)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable) throw new Exception503();

            var jsonContent = response.Content;
            var stations = JsonSerializer.Deserialize<StationDTO[]>(jsonContent);
            return stations;
        }
        /// <summary>
        /// Updates all stations
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="newStations"></param>
        public void UpdateAllStations(string serverPath, string token, StationDTO[] newStations)
        {
            var client = new RestClient(serverPath + nmspace)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            if (newStations != null)
            {
                string body = JsonSerializer.Serialize(newStations);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.BadRequest) throw new Exception400();
            else if (response.StatusCode == HttpStatusCode.Unauthorized) throw new Exception401();
            else if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
        }
        /// <summary>
        /// Return station of given id
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StationDTO GetStation(string serverPath, int id)
        {
            var client = new RestClient(serverPath + nmspace + @"/" + id.ToString())
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
            else if (response.StatusCode == HttpStatusCode.ServiceUnavailable) throw new Exception503();
            var jsonContent = response.Content;

            var station = JsonSerializer.Deserialize<StationDTO>(jsonContent);
            return station;
        }
        /// <summary>
        /// Reports station as possibly broken
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        public void ReportStation(string serverPath, int id)
        {
            var client = new RestClient(serverPath + nmspace + @"/" + id.ToString() + @"/report")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable) throw new Exception503();
            else if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
        }
        /// <summary>
        /// Sets station as broken
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        public void SetStationAsBroken(string serverPath, string token, int id)
        {
            var client = new RestClient(serverPath + nmspace + @"/" + id.ToString() + @"/broken")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new Exception401();
            else if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
        }
        /// <summary>
        /// Sets station as working
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        public void SetStationAsWorking(string serverPath, string token, int id)
        {
            var client = new RestClient(serverPath + nmspace + @"/" + id.ToString() + @"/working")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized) throw new Exception401();
            else if (response.StatusCode == HttpStatusCode.NotFound) throw new Exception404();
        }
    }
}
