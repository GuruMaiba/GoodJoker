using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoodJoker.Models
{
    public class EditContact
    {
        public string SiteName { get; set; }
        public string SiteLink { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<LinkStudio> Link { get; set; }
    }
}