namespace FreeturiloWebApi.DTO
{
    public class StationDTO : LocationDTO
    {
        public int Id  { get; set; }
        public int BikeRacks  { get; set; }
        public int Bikes { get; set; }
        public int State { get; set; }
    }
}
