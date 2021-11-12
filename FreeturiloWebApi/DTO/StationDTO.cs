using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class StationDTO : LocationDTO
    {
        [JsonPropertyName("id")]
        public int Id  { get; set; }
        [JsonPropertyName("bikeRacks")]
        public int BikeRacks  { get; set; }
        [JsonPropertyName("bikes")]
        public int Bikes { get; set; }
        [JsonPropertyName("state")]
        public int State { get; set; }
    }
}
