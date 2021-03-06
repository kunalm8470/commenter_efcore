using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class UpdateCommentDto
    {
        [JsonPropertyName("comment_id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("body")]
        [Required]
        [MaxLength(1024)]
        public string Body { get; set; }

        [JsonPropertyName("user_id")]
        [Required]
        public int UserId { get; set; }

        [JsonPropertyName("post_id")]
        [Required]
        public int PostId { get; set; }
    }
}
