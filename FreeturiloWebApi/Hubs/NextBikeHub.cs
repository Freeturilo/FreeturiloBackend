using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Hubs
{
    public class NextBikeHub : Hub
    {
        public Task UpdateStation(int stationId, double lat, double lon, int availableBikes)
        {
            return Clients.Others.SendAsync("updateStation", stationId, lat, lon, availableBikes);
        }
    }
}
