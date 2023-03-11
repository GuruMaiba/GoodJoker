using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class Warning
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        [DefaultValue(false)]
        public bool Close { get; set; }

        public string Type { get; set; }

        public virtual User User { get; set; }

        public virtual Studio Studio { get; set; }
    }
}