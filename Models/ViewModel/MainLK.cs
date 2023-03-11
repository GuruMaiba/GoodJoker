using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class MainLK
    {
        public bool HaveReview { get; set; }
        public ICollection<WinnersEverydayLott> EverydayPrize { get; set; }
        public ICollection<Prize> LottPrize { get; set; }
        public ICollection<RefreshWinner> LottRefresh { get; set; }
        public ICollection<Lottery> Lott { get; set; }
        public ICollection<EverydayLott> Everyday { get; set; }
    }
}