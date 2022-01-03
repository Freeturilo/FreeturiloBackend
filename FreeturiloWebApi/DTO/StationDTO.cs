using System.Text.Json.Serialization;

namespace FreeturiloWebApi.DTO
{
    public class StationDTO : LocationDTO
    {
        /// <summary>
        /// Station id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id  { get; set; }
        /// <summary>
        /// Station state
        /// </summary>
        [JsonPropertyName("state")]
        public int State { get; set; }
    }
}
