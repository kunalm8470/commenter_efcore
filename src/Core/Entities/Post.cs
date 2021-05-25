using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Post : BaseEntity<int>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Body { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public IReadOnlyList<Comment> Comments { get; set; }
    }
}
