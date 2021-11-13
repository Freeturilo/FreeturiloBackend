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
        public static bool Compare(markers nextbikeData, StationDTO[] freeturiloData, out List<StationDTO> toBeAdded, out List<StationDTO> toBeUpdated)
        {
            toBeAdded = new List<StationDTO>();
            toBeUpdated = new List<StationDTO>();

            foreach (var station in nextbikeData.country.city.place)
            {
                var stationDTO = freeturiloData.Where(s => s.Id == station.uid).FirstOrDefault();
                if (stationDTO == null)
                {
                    toBeAdded.Add(new StationDTO()
                    {
                        Id = station.uid,
                        Latitude = (double)station.lat,
                        Longitude = (double)station.lng,
                        Name = station.name,
                        Bikes = station.bikes_available_to_rent,
                        BikeRacks = station.free_racks,
                        State = 0,
                    });
                }
                else if (stationDTO.Bikes != station.bikes_available_to_rent || stationDTO.BikeRacks != station.free_racks)
                {
                    stationDTO.Bikes = station.bikes_available_to_rent;
                    stationDTO.BikeRacks = station.free_racks;

                    toBeUpdated.Add(stationDTO);
                }
            }

            return nextbikeData.country.city.place.Length != freeturiloData.Length;
        }
    }
}
