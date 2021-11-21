using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeturiloWebApi.DTO.GoogleDTO
{
    public class ViaWaypoint
    {
        [JsonPropertyName("location")]
        public Coords Location { get; set; }
        [JsonPropertyName("step_index")]
        public int StepIndex { get; set; }
        [JsonPropertyName("step_interpolation")]
        public double StepInterpolation { get; set; }
    }
    public class GeocodedWaypoint
    {
        [JsonPropertyName("geocoder_status")]
        public string Status { get; set; }
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
    }
    public class Coords
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }
    public class Bounds
    {
        [JsonPropertyName("northeast")]
        public Coords Northeast { get; set; }
        [JsonPropertyName("southwest")]
        public Coords Southwest { get; set; }
    }
    public class TextValue
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
    public class Polyline
    {
        [JsonPropertyName("points")]
        public string Points { get; set; }
    }
    public class Step
    {
        [JsonPropertyName("distance")]
        public TextValue Distance { get; set; }
        [JsonPropertyName("duration")]
        public TextValue Duration { get; set; }
        [JsonPropertyName("end_location")]
        public Coords EndLocation { get; set; }
        [JsonPropertyName("start_location")]
        public Coords StartLocation { get; set; }
        [JsonPropertyName("html_instructions")]
        public string HtmlInstructions { get; set; }
        [JsonPropertyName("travel_mode")]
        public string TravelMode { get; set; }
        [JsonPropertyName("polyline")]
        public Polyline Polyline { get; set; }

    }
    public class Leg
    {
        [JsonPropertyName("distance")]
        public TextValue Distance { get; set; }
        [JsonPropertyName("duration")]
        public TextValue Duration { get; set; }
        [JsonPropertyName("end_address")]
        public string EndAddress { get; set; }
        [JsonPropertyName("end_location")]
        public Coords EndLocation { get; set; }
        [JsonPropertyName("start_address")]
        public string StartAddress { get; set; }
        [JsonPropertyName("start_location")]
        public Coords StartLocation { get; set; }
        [JsonPropertyName("steps")]
        public Step[] Steps { get; set; }
        [JsonPropertyName("traffic_speed_entry")]
        public List<string> TrafficSpeedEntry { get; set; }
        [JsonPropertyName("via_waypoint")]
        public List<ViaWaypoint> ViaWaypoint { get; set; }
    }
    public class Route
    {
        [JsonPropertyName("bounds")]
        public Bounds Bounds { get; set; }
        [JsonPropertyName("copyrights")]
        public string Copyrights { get; set; }
        [JsonPropertyName("legs")]
        public Leg[] Legs { get; set; }
        [JsonPropertyName("overview_polyline")]
        public Polyline OverviewPolyline { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("warnings")]
        public List<string> Warnings { get; set; }
        [JsonPropertyName("waypoint_order")]
        public int[] WaypointOrder { get; set; }
    }
    public class DirectionDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("geocoded_waypoints")]
        public GeocodedWaypoint[] GeocodedWaypoints { get; set; }
        [JsonPropertyName("routes")]
        public Route[] Routes { get; set; }
    }
}
