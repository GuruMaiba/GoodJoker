using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class LocalLimiter
    {
        public ICollection<RegionCity> Regions { get; set; } = new List<RegionCity>();
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}