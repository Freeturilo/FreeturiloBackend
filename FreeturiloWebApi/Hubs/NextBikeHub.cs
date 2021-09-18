using FreeturiloWebApi.Hubs.HubClients;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Hubs
{
    public class NextBikeHub : Hub<INextBikeHubClient>
    {
        public Task UpdateStation(string station, int availableBikes)
        {
            return Clients.All.UpdateStation(station, availableBikes);
        }
    }
}
