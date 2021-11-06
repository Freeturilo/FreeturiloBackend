namespace FreeturiloWebApi.DTO
{
    public class RouteParametersDTO
    {
        public LocationDTO Start  { get; set; }
        public LocationDTO End  { get; set; }
        public LocationDTO[] Stops { get; set; }
        public int Criterion { get; set; }
    }
}
