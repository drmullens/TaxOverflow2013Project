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

        public ActionResult PostQuestion()
        {
            ViewBag.Message = "Post a question";

            return View();
        }

        public ActionResult ViewOpenQuestions()
        {
            ViewBag.Message = "View unanswered questions";

            return View();
        }

        public ActionResult ViewQuestions()
        {
            ViewBag.Message = "View answered questions";

            return View();
        }
    }
}