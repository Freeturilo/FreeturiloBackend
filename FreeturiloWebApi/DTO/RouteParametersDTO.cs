using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class RouteParametersDTO
    {
        [JsonPropertyName("start")]
        public LocationDTO Start  { get; set; }
        [JsonPropertyName("end")]
        public LocationDTO End  { get; set; }
        [JsonPropertyName("stops")]
        public LocationDTO[] Stops { get; set; }
        [JsonPropertyName("criterion")]
        public int Criterion { get; set; }
    }
}
