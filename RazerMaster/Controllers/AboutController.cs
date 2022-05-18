using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RazerMaster.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Story()
        {
            return View();
        }
        // GET: Store
        public ActionResult Store()
        {
            return View();
        }
    }
}