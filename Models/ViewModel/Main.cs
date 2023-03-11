using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class Main
    {
        public ICollection<News> News { get; set; }
        public ICollection<Lottery> Lott { get; set; }
        public ICollection<Assess> Reviews { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}