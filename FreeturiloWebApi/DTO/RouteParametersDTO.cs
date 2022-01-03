using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class RouteParametersDTO
    {
        /// <summary>
        /// Start location
        /// </summary>
        [JsonPropertyName("start")]
        public LocationDTO Start  { get; set; }
        /// <summary>
        /// End location
        /// </summary>
        [JsonPropertyName("end")]
        public LocationDTO End  { get; set; }
        /// <summary>
        /// Stops of route
        /// </summary>
        [JsonPropertyName("stops")]
        public LocationDTO[] Stops { get; set; }
        /// <summary>
        /// Criterion of route
        /// </summary>
        [JsonPropertyName("criterion")]
        public int Criterion { get; set; }
    }
}
