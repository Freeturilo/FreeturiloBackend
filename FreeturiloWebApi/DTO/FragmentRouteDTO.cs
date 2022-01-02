using FreeturiloWebApi.DTO.GoogleDTO;
using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class FragmentRouteDTO
    {
        [JsonPropertyName("parameters")]
        public RouteParametersDTO Parameters  { get; set; }
        [JsonPropertyName("cost")]
        public double Cost  { get; set; }
        [JsonPropertyName("time")]
        public int Time { get; set; }
        [JsonPropertyName("waypoints")]
        public LocationDTO[] Waypoints { get; set; }
        [JsonPropertyName("directionsRoute")]
        public Route DirectionsRoute { get; set; }
    }
}
