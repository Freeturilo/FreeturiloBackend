using System.ComponentModel.DataAnnotations;

namespace FreeturiloWebApi.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public string Name { get; set; }
        public int AvailableBikes { get; set; }
        public int AvailableRacks { get; set; }
        public int Reports { get; set; }
    }
}
