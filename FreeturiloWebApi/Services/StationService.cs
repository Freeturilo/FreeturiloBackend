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
            var stations = new List<Station>();
            foreach(var stationDTO in newStations)
            {
                var station = _context.Stations.Where(s => s.Id == stationDTO.Id).FirstOrDefault();
                if (station == null) throw new Exception404();
                stations.Add(station);
            }

            for(int i=0; i<newStations.Length; i++)
            {
                var station = stations[i];
                var stationDTO = newStations[i];

                station.AvailableBikes = stationDTO.Bikes;
                station.AvailableRacks = stationDTO.BikeRacks;
            }
            
            _context.SaveChanges();
        }
    }
}
