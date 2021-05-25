using System;
using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class CommentResponseDto
    {
        [JsonPropertyName("comment_id")]
        public int Id { get; set; }
        
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("post")]
        public UserPost Post { get; set; }

        [JsonPropertyName("commenter")]
        public UserResponseBase Commenter { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
