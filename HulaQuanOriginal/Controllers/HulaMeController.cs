using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HulaQuanOriginal.Controllers
{
    public class HulaMeController : Controller
    {
        // GET: HulaMe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get(int id)
        {
            return View();
        }
    }
}