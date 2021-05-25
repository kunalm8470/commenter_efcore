using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Comment : BaseEntity<int>
    {
        [Required]
        [MaxLength(1024)]
        public string Body { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
