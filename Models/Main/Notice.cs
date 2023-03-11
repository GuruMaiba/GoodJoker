using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class Notice
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        [DefaultValue(false)]
        public bool Close { get; set; }
        
        public virtual ICollection<User> Users { get; set; }

        public Notice()
        {
            Users = new List<User>();
        }
    }
}