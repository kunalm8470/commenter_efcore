using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class SearchParams
    {
        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
    }
}
