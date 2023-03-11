using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class InfoSite
    {
        [Key]
        public string Token { get; set; }
        public int CountRegUser { get; set; }
        public int CountArchLott { get; set; }
        public double AverageAssess { get; set; }
        public int CountAssessUser { get; set; }
    }
}