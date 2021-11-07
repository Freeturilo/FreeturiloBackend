using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public interface IStationService
    {
        Station[] GetAllStations();
        Station AddNewStation(Station newStation);
        void UpdateAllStations(Station[] newStations);
        Station GetStation(int stationId);
        void UpdateStation(int stationId, Station newStation);
        void ReportStation(int stationId);
        void SetStationAsBroken(int stationId);
        void SetStationAsWorking(int stationId);
    }
}
