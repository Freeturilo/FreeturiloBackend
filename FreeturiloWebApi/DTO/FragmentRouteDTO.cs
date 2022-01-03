using FreeturiloWebApi.DTO.GoogleDTO;
using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class FragmentRouteDTO
    {
        /// <summary>
        /// Parameters of route
        /// </summary>
        [JsonPropertyName("parameters")]
        public RouteParametersDTO Parameters  { get; set; }
        /// <summary>
        /// Cost of route
        /// </summary>
        [JsonPropertyName("cost")]
        public double Cost  { get; set; }
        /// <summary>
        /// Time of route
        /// </summary>
        [JsonPropertyName("time")]
        public int Time { get; set; }
        /// <summary>
        /// Waypoints of route
        /// </summary>
        [JsonPropertyName("waypoints")]
        public LocationDTO[] Waypoints { get; set; }
        /// <summary>
        /// Google Maps route
        /// </summary>
        [JsonPropertyName("directionsRoute")]
        public Route DirectionsRoute { get; set; }
    }
}
