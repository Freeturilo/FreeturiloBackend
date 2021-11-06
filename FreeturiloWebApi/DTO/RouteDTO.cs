namespace FreeturiloWebApi.DTO
{
    public class RouteDTO
    {
        public RouteParametersDTO Parameters  { get; set; }
        public double Cost  { get; set; }
        public LocationDTO[] Waypoints { get; set; }
        public object DirectionsRoute { get; set; }
    }
}
