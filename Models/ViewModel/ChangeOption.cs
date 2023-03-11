using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class ChangeOption
    {
        public string Nick { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NowPass { get; set; }
        public string NewPass { get; set; }
        public string ConfPass { get; set; }
        public int City { get; set; }
        public int BigCity { get; set; }
        public string ExactAddress { get; set; }
        public string GeoMap { get; set; }
        public DateTime Birthday { get; set; }
        public byte Gender { get; set; }
    }
}