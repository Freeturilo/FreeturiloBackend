using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Models
{
    public class Station
    {
        public double? Lng { get; set; }
        public double? Lat { get; set; }
        public string Name { get; set; }
        [Key]
        public int Id { get; set; }
        public int AvailableBikes { get; set; }
    }
}
