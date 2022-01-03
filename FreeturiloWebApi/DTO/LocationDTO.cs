using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class LocationDTO
    {
        /// <summary>
        /// Longitude of location
        /// </summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set;}
        /// <summary>
        /// Latitude of location
        /// </summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set;}
        /// <summary>
        /// Name of location
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set;}
        /// <summary>
        /// Type of location
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Number of racks on station
        /// </summary>
        [JsonPropertyName("bikeRacks")]
        public int? BikeRacks { get; set; }
        /// <summary>
        /// Number of bikes on station
        /// </summary>
        [JsonPropertyName("bikes")]
        public int? Bikes { get; set; }
    }
}
