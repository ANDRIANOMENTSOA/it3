using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;



namespace WebApplication1.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Startup()
        {
          
            ViewBag.Message = "Your startup page.";
            return View();
        }

        public ActionResult AutreStartup()
        {
            return PartialView();
        }

        public ActionResult Query_ADMACM()
        {
            ViewBag.Message = "Your Query ADMACM page.";
            return View();
        }
        public ActionResult Create_ADMACM()
        {
            ViewBag.Message = "Your Create ADMACM page.";
            return View();
        }

        public ActionResult Sales_Data_Upload()
        {
            ViewBag.Message = "Your Sales_Data_Upload page.";
            return View();
        }
        public ActionResult Total_Amount_Per_PSR()
        {
            ViewBag.Message = "Your Total Amount Per PSR page.";
            return View();
        }
        public ActionResult Key_Controlling_Data()
        {
            ViewBag.Message = "Your Key Controlling Data page.";
            return View();
        }
        public ActionResult Key_Fields_Breakdown()
        {
            ViewBag.Message = "Your Key Fields Breakdown page.";
            return View();
        }
        public ActionResult Issues()
        {
            ViewBag.Message = "Your Issues page.";
            return View();
        }
        public ActionResult Cancellation()
        {
            ViewBag.Message = "Your Cancellation page.";
            return View();
        }
        public ActionResult ACMs()
        {
            ViewBag.Message = "Your ACMs page.";
            return View();
        }
        public ActionResult ADMs()
        {
            ViewBag.Message = "Your ADMs page.";
            return View();
        }
        public ActionResult List_of_Refunds()
        {
            ViewBag.Message = "Your List of Refunds page.";
            return View();
        }
        public ActionResult Refunds() 
        {
            ViewBag.Message = "Your Refunds page.";
            return View();
        }
        public ActionResult Refunds_OWN()
        {
            ViewBag.Message = "Your Refunds page.";
            return View();
        }
        public ActionResult Commission_Reclaim()
        {
            ViewBag.Message = "Your Commission_Reclaim page.";
            return View();
        }
        
        public ActionResult Exchanges()
        {
            ViewBag.Message = "Your Exchanges page.";
            return View();
        }
        public ActionResult ISC_Reclaim()
        {
            ViewBag.Message = "Your ISC Reclaim page.";
            return View();
        }
        public ActionResult Retroactive_Adjustments()
        {
            ViewBag.Message = "Your Retroactive Adjustments page.";
            return View();
        }
        public ActionResult FIMs()
        {
            ViewBag.Message = "Your FIMs page.";
            return View();
        }
        public ActionResult RET_Anciliaries()
        {
            ViewBag.Message = "Your ISC Reclaim page.";
            return View();
        }
        public ActionResult Welcom()
        {
            return PartialView();

        }

    }
}