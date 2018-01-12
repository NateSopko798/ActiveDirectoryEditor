using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiActiveSorbDirectory.Controllers
{
    public class EditUserController : Controller
    {
        // GET: EditUser
        public ActionResult Index()
        {

            //to assign logon script myADEntry.Properties["scriptPath"].Insert(0, "login.vbs");
            return View();
        }
    }
}