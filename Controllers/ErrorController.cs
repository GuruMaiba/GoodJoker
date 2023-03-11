using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoodJoker.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult ServerDontWork()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}