using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class CustomisedController : Controller
    {
        // GET: Customised
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}