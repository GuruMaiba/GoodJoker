using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJoker.Models
{
    public class Assess
    {
        [Key, ForeignKey("User")]
        public int Id { get; set; }
        public float Rate { get; set; }
        public string Text { get; set; }
        public string ReplyText { get; set; }
        public bool OnTheMain { get; set; }
        public bool Publish { get; set; }
        public virtual User User { get; set; }
    }
}