using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Services
{
    public class StationService : IStationService
    {
        private readonly FreeturiloContext _context;
        public StationService(FreeturiloContext context)
        {
            _context = context;
        }
        public Station AddNewStation(Station newStation)
        {
            var station = _context.Stations.Where(s => s.Id == newStation.Id).FirstOrDefault();
            if (station != null) throw new Exception400("Istnieje już stacja o podanym ID");

            _context.Stations.Add(newStation);
            _context.SaveChanges();
            return newStation;
        }

        public Station[] GetAllStations()
        {
            var stations = _context.Stations.ToArray();
            return stations;
        }

        public Station GetStation(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station is null) throw new Exception404("Nie znaleziono stacji o podanym ID");

            return station;
        }

        public void ReportStation(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if(station == null) throw new Exception404("Nie znaleziono stacji o podanym ID");
            station.Reports++;
            if (station.State == 0) station.State = 1;
            _context.SaveChanges();
            //TODO email to administartors
        }

        public void SetStationAsBroken(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404("Nie znaleziono stacji o podanym ID");
            station.State = 2;
            _context.SaveChanges();
        }

        public void SetStationAsWorking(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404("Nie znaleziono stacji o podanym ID");
            station.State = 0;
            station.Reports = 0;
            _context.SaveChanges();
        }

        public void UpdateAllStations(Station[] newStations)
        {
            //TODO do it better
            var routes = _context.Routes.ToArray();
            _context.Routes.RemoveRange(routes);

            var stations = _context.Stations.ToArray();
            _context.Stations.RemoveRange(stations);

            _context.Stations.AddRange(newStations);
            //TODO fill Routes Table
            
            _context.SaveChanges();
        }

        public void UpdateStation(int stationId, Station newStation)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404("Nie znaleziono stacji o podanym ID");

            station.AvailableRacks = newStation.AvailableRacks;
            station.AvailableBikes = newStation.AvailableBikes;

            _context.SaveChanges();
        }
    }
}
