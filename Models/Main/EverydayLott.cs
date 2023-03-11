using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJoker.Models
{
    public class EverydayLott
    {
        public int Id { get; set; }

        public string Cover { get; set; }

        public string FromWhom { get; set; }

        public DateTime Holding { get; set; }

        public string Description { get; set; }

        public string Prize { get; set; }

        public bool Publish { get; set; }

        [InverseProperty("Everyday")]
        public virtual ICollection<User> Participants { get; set; }

        // Лайки
        [InverseProperty("LikesEveryday")]
        public virtual ICollection<User> Likes { get; set; }

        public virtual ICollection<WinnersEverydayLott> Winners { get; set; }

        public EverydayLott()
        {
            Participants = new List<User>();
            Likes = new List<User>();
            Winners = new List<WinnersEverydayLott>();
        }
    }

    public class WinnersEverydayLott
    {
        public int Id { get; set; }

        public string Prize { get; set; }

        public bool Confirm { get; set; }

        public bool View { get; set; }

        public DateTime HoldingLott { get; set; }

        public virtual User Winner { get; set; }

        public virtual EverydayLott Lottery { get; set; }
    }
}