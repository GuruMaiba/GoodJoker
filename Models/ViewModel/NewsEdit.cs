using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoodJoker.Models
{
    public class NewsEdit
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Tags { get; set; }
        public int Publish { get; set; }
    }
}