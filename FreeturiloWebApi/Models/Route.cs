using System.ComponentModel.DataAnnotations;

namespace FreeturiloWebApi.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        public int StartId { get; set; }
        public int StopId { get; set; }
        public int Time { get; set; }
        public double Cost { get; set; }
        public string RouteJSON { get; set; }
    }
}
