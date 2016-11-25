using CompressR.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompressR.Sample.Controllers
{
    public class HomeController : Controller
    {
        [Compress]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View("index");
        }
        public ActionResult NoZip()
        {
            ViewBag.Title = "Home Page";

            return View("index");
        }
        [Gzip]
        public ActionResult Gzip()
        {
            ViewBag.Title = "Home Page";

            return View("index");
        }
        [Deflate]
        public ActionResult Deflate()
        {
            ViewBag.Title = "Home Page";

            return View("index");
        }
    }
}