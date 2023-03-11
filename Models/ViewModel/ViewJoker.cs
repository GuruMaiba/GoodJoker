using System;
using System.Collections.Generic;

namespace GoodJoker.Models
{
    public class ViewJoker
    {
        public User User { get; set; }
        public ICollection<PrizesProfile> Prizes { get; set; }
    }

    public class PrizesProfile
    {
        public DateTime DataWin { get; set; }
        public int Place { get; set; }
        public string Prize { get; set; }
    }
}