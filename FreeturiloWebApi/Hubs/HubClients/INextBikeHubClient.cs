using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Hubs.HubClients
{
    public interface INextBikeHubClient
    {
        Task UpdateStation(string station, int availableBikes);
    }
}
