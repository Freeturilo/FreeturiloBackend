using FreeturiloWebApi.DTO.GoogleDTO;

namespace FreeturiloWebApi.DTO
{
    public class RouteDTO
    {
        public RouteParametersDTO Parameters  { get; set; }
        public double Cost  { get; set; }
        public int Time { get; set; }
        public LocationDTO[] Waypoints { get; set; }
        public DirectionDTO DirectionsRoute { get; set; }
    }
}
