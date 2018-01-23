using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiActiveSorbDirectory.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Index(String source)
        {
            ViewBag.source = source;
            return View();
        }

        //POST: Redirect again
        [HttpPost]
        public ActionResult RedirectMe()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}