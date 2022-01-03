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
        /// <summary>
        /// Returns all stations
        /// </summary>
        /// <returns></returns>
        StationDTO[] GetAllStations();
        /// <summary>
        /// Updates all stations
        /// </summary>
        /// <param name="newStations">Collection of stations to be updated</param>
        void UpdateAllStations(StationDTO[] newStations);
        /// <summary>
        /// Returns station of given id
        /// </summary>
        /// <param name="stationId">Station id</param>
        /// <returns></returns>
        StationDTO GetStation(int stationId);
        /// <summary>
        /// Reports station
        /// </summary>
        /// <param name="stationId">Station id</param>
        void ReportStation(int stationId);
        /// <summary>
        /// Sets station as broken
        /// </summary>
        /// <param name="stationId">Station id</param>
        void SetStationAsBroken(int stationId);
        /// <summary>
        /// Sets station as working
        /// </summary>
        /// <param name="stationId">Station id</param>
        void SetStationAsWorking(int stationId);
    }
}
