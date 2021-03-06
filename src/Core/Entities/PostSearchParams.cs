using System;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class PostSearchParams : SearchParams
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime? CreatedOn { get; set; }
    }
}
