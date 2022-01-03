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
    public interface IStationMethods
    {
        /// <summary>
        /// Returns all stations
        /// </summary>
        /// <param name="serverPath"></param>
        /// <returns></returns>
        StationDTO[] GetAllStations(string serverPath);
        /// <summary>
        /// Updates all stations
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="newStations"></param>
        void UpdateAllStations(string serverPath, string token, StationDTO[] newStations);
        /// <summary>
        /// Returns station of given id
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        StationDTO GetStation(string serverPath, int id);
        /// <summary>
        /// Reports station as possibly broken
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="id"></param>
        void ReportStation(string serverPath, int id);
        /// <summary>
        /// Sets station as broken
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        void SetStationAsBroken(string serverPath, string token, int id);
        /// <summary>
        /// Sets station as working
        /// </summary>
        /// <param name="serverPath"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        void SetStationAsWorking(string serverPath, string token, int id);
    }
}
