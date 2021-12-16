using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class StationDTO : LocationDTO
    {
        [JsonPropertyName("id")]
        public int Id  { get; set; }
        [JsonPropertyName("state")]
        public int State { get; set; }
    }
}
