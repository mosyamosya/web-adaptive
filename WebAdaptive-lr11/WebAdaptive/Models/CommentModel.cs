using System.Text.Json.Serialization;

namespace WebAdaptive.Models
{
    public class CommentModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Content { get; set; }
    }
}
