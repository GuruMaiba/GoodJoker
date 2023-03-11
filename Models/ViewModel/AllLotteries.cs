using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class AllLotteries
    {
        public ICollection<Lottery> EndLott { get; set; }
        public ICollection<Lottery> OtherLott { get; set; }
        public ICollection<Lottery> GJLott { get; set; }
        public ICollection<EverydayLott> Everyday { get; set; }
        public int AllCountGJ { get; set; }
        public int AllCountUser { get; set; }
        public int HMS { get; set; }
    }
}