using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxOverflow2013.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Question()
        {
            ViewBag.Message = "Post a Question";

            return View();
        }

        public ActionResult QuestionList()
        {
            ViewBag.Message = "View Questions";

            return View();
        }

        public ActionResult View()
        {
            ViewBag.Message = "View a question";

            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Message = "View Search Results";

            return View();
        }

        public ActionResult Answer()
        {
            ViewBag.Message = "Post an Answer";

            return View();
        }

        public ActionResult Comment()
        {
            ViewBag.Message = "Post a Comment";

            return View();
        }
    }
}