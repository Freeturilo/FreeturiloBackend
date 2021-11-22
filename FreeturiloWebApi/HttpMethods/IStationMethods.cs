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
        StationDTO[] GetAllStations(string serverPath);

        void UpdateAllStations(string serverPath, string token, StationDTO[] newStations);

        StationDTO GetStation(string serverPath, int id);

        void ReportStation(string serverPath, int id);
        void SetStationAsBroken(string serverPath, string token, int id);
        void SetStationAsWorking(string serverPath, string token, int id);
    }
}
