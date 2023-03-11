using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class SocialNetwork
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SocialId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}