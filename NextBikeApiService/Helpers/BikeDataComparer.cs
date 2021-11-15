using FreeturiloWebApi.DTO;
using NextBikeDataParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBikeApiService.Helpers
{
    static class BikeDataComparer
    {
        public static IEnumerable<StationDTO> Compare(markers nextbikeData, StationDTO[] freeturiloData)
        {
            var toBeUpdated = new List<StationDTO>();

            foreach (var station in freeturiloData)
            {
                var nbStation = nextbikeData.country.city.place.Where(s => s.uid == station.Id).FirstOrDefault();
                if (nbStation == null)
                {
                    continue;
                }
                else if (station.Bikes != nbStation.bikes_available_to_rent || station.BikeRacks != nbStation.free_racks)
                {
                    station.Bikes = nbStation.bikes_available_to_rent;
                    station.BikeRacks = nbStation.free_racks;

                    toBeUpdated.Add(station);
                }
            }

            return toBeUpdated;
        }
    }
}
