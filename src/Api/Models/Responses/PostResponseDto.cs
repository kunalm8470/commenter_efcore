using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class PostResponseDto
    {
        [JsonPropertyName("post_id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("author")]
        public UserResponseBase User { get; set; }

        [JsonPropertyName("comments")]
        public IReadOnlyList<PostComment> Comments { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PostComment : UserComment
    {
        [JsonPropertyName("commenter")]
        public UserResponseBase Commenter { get; set; }
    }
}
