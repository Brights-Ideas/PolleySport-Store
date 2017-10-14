using PolleySportStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIdentityApp.Controllers
{
    [RequireHttps]
    public class StoreHomeController : Controller
    {
        readonly StoreEntities storeDB = new StoreEntities();

        public ActionResult Index()
        {
           // ViewBag.UserCount = storeDB..Products.ToList().Count;
            ViewBag.ProductCount = storeDB.Products.ToList().Count;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}