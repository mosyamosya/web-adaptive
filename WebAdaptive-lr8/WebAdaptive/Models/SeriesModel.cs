using System.Text.Json.Serialization;

namespace WebAdaptive.Models
{
    public class SeriesModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NumberOfEpisodes { get; set; } 
        public int SeasonCount { get; set; } 
        public bool IsOngoing { get; set; }
    }
}
