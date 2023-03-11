using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class NewsMain
    {
        public ICollection<Lottery> Lotteries { get; set; }
        public ICollection<Prize> Winners { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<News> News { get; set; }
    }
}