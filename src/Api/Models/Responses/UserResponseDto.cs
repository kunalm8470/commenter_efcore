using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Models.Responses
{
    public class UserResponseBase
    {
        [JsonPropertyName("user_id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("gender_abbreviation")]
        public char GenderAbbreviation { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }      

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class UserResponseDto : UserResponseBase
    {
        [JsonPropertyName("posts")]
        public IReadOnlyList<UserPost> Posts { get; set; }

        [JsonPropertyName("comments")]
        public IReadOnlyList<UserComment> Comments { get; set; }
    }

    public class UserPost
    {
        [JsonPropertyName("post_id")]
        public int PostId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class UserComment
    {
        [JsonPropertyName("comment_id")]
        public int CommentId { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
