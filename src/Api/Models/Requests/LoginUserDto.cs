using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models.Requests
{
    public class LoginUserDto
    {
        [JsonPropertyName("login")]
        [Required]
        [MaxLength(256)]
        public string Login { get; set; }
        
        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }
    }
}
