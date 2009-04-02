using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Blackboard.Controllers
{
    public class BlackboardController : Controller
    {
        //
        // GET: /Blackboard/

        public ActionResult Index()
        {
            return View();
        }

    }
}
