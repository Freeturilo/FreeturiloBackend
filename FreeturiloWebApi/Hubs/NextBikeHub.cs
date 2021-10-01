using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Hubs
{
    public class NextBikeHub : Hub
    {
        private readonly FreeturiloContext _context;
        public NextBikeHub(FreeturiloContext context)
        {
            _context = context;
        }
        public Task UpdateStations(IEnumerable<Station> newStations)
        {
            foreach (var newsStation in newStations)
            {
                var station = _context.Stations.Where(s => s.Id == newsStation.Id).FirstOrDefault();
                if (station == null)
                {
                    _context.Stations.Add(newsStation);
                }
                else
                {
                    station.AvailableBikes = newsStation.AvailableBikes;
                }
            }

            _context.SaveChanges();

            var stations = _context.Stations.ToArray();
            return Clients.Others.SendAsync("updateStations", stations);
        }

        public Task GetAllStations()
        {
            var stations = _context.Stations.ToArray();
            return Clients.Caller.SendAsync("getAllStations", stations);
        }
    }
}
