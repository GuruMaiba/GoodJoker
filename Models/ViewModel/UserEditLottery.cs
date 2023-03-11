using System;
using System.Collections.Generic;

namespace GoodJoker.Models
{
    public class UserEditLottery
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public string PrizeName { get; set; }
        public int PrizePlace { get; set; }
        public string YoutubeId { get; set; }
        public DateTime Date { get; set; }
        public int GMT { get; set; }
        public int CountUser { get; set; }
        public bool EditCountUser { get; set; } = false;
        public ICollection<LinkLottery> Link { get; set; } = new List<LinkLottery>();
    }
}