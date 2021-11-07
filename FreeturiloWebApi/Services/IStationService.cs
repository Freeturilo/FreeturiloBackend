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
        StationDTO AddNewStation(StationDTO newStation);
        void UpdateAllStations(StationDTO[] newStations);
        StationDTO GetStation(int stationId);
        void UpdateStation(int stationId, StationDTO newStation);
        void ReportStation(int stationId);
        void SetStationAsBroken(int stationId);
        void SetStationAsWorking(int stationId);
    }
}
