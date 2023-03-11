using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class AdminLottery
    {
        public ICollection<Lottery> Lotteries { get; set; }
        public ICollection<EverydayLott> Everyday { get; set; }
    }
}