using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.DTO
{
    public class StationDTO
    {
        public double? Lng { get; set;}
        public double? Lat { get; set;}
        public string Name { get; set;}
        public int Id { get; set;}
        public int AvailableBikes  { get; set; }
    }
}
