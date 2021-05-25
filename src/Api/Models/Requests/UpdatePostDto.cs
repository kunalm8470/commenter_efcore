using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class UpdatePostDto
    {
        [JsonPropertyName("post_id")]
        [Required]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        [Required]
        [MaxLength(1024)]
        public string Body { get; set; }

        [JsonPropertyName("user_id")]
        [Required]
        public int UserId { get; set; }
    }
}
