using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public interface IStationService
    {
        StationDTO[] GetAllStations();
        void UpdateAllStations(StationDTO[] newStations);
        StationDTO GetStation(int stationId);
        void ReportStation(int stationId);
        void SetStationAsBroken(int stationId);
        void SetStationAsWorking(int stationId);
    }
}
