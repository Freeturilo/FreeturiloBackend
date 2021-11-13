using AutoMapper;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
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
        private readonly IMapper _mapper;

        public StationService(FreeturiloContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }
        public StationDTO AddNewStation(StationDTO newStation)
        {
            if (newStation == null) throw new Exception400();
            var existingStation = _context.Stations.Where(s => s.Id == newStation.Id).FirstOrDefault();
            if (existingStation != null) throw new Exception400();

            var station = _mapper.Map<Station>(newStation);
            _context.Stations.Add(station);
            _context.SaveChanges();
            return newStation;
        }

        public StationDTO[] GetAllStations()
        {
            var stations = _context.Stations.ToArray();
            var stationDTOs = _mapper.Map<StationDTO[]>(stations);
            return stationDTOs;
        }

        public StationDTO GetStation(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station is null) throw new Exception404();

            var stationDTO = _mapper.Map<StationDTO>(station);

            return stationDTO;
        }

        public void ReportStation(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if(station == null) throw new Exception404();
            station.Reports++;
            if (station.State == 0) station.State = 1;
            _context.SaveChanges();

            var administrators = _context.Administrators.ToArray();
            foreach(var admin in administrators)
            {
                if(admin.NotifyThreshold == station.Reports)
                {
                    GmailAPIHandler.SendEmail(admin, station);
                }
            }
        }

        public void SetStationAsBroken(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404();
            station.State = 2;
            _context.SaveChanges();
        }

        public void SetStationAsWorking(int stationId)
        {
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404();
            station.State = 0;
            station.Reports = 0;
            _context.SaveChanges();
        }

        public void UpdateAllStations(StationDTO[] newStations)
        {
            if (newStations == null) throw new Exception400();
            
            //TODO do it better
            var routes = _context.Routes.ToArray();
            _context.Routes.RemoveRange(routes);

            var stations = _context.Stations.ToArray();
            _context.Stations.RemoveRange(stations);

            var newStationDTOs = _mapper.Map<Station[]>(newStations);
            _context.Stations.AddRange(newStationDTOs);
            //TODO fill Routes Table
            
            _context.SaveChanges();
        }

        public void UpdateStation(int stationId, StationDTO newStation)
        {
            if (newStation == null) throw new Exception400();
            var station = _context.Stations.Where(s => s.Id == stationId).FirstOrDefault();
            if (station == null) throw new Exception404();

            station.AvailableRacks = newStation.BikeRacks;
            station.AvailableBikes = newStation.Bikes;

            _context.SaveChanges();
        }
    }
}
