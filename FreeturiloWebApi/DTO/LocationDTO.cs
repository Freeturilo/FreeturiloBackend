using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class LocationDTO
    {
        [JsonPropertyName("longitude")]
        public double Longitude { get; set;}
        [JsonPropertyName("latitude")]
        public double Latitude { get; set;}
        [JsonPropertyName("name")]
        public string Name { get; set;}
    }
}
