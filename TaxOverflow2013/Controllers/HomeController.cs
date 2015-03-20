using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxOverflow2013.Models;

namespace TaxOverflow2013.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeModel.MockIndexModel());
        }

        public ActionResult Question()
        {
            ViewBag.Message = "Post a Question";

            return View();
        }

        public ActionResult QuestionList()
        {
            ViewBag.Message = "View Questions";

            return View(new HomeModel.MockIndexModel());
        }

        public ActionResult ViewQuestion()
        {
            ViewBag.Message = "View a question";

            if (Request["vote"] != null && !String.IsNullOrWhiteSpace(Request["vote"]))
            {
                return View(new TaxOverflow2013.Models.HomeModel.MockViewQuestion((Int32.Parse(Request["question_id"])), Int32.Parse(Request["vote"]), Convert.ToChar(Request["math"])));
            }
            else if (Request["question_id"] != null && !String.IsNullOrWhiteSpace(Request["question_id"]))
            {
                return View(new HomeModel.MockViewQuestion(Int32.Parse(Request["question_id"])));
            }
            else
            {
                Response.Redirect("~/");
                return null;
            }
        }

        public ActionResult Search()
        {
            ViewBag.Message = "View Search Results";

            return View(new HomeModel.MockIndexModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult btnClick(string txtQuestion, string ddlCategory)
        {
            string data = txtQuestion;

            //Return new QuestionID
            
            return View("ViewQuestion", new HomeModel.MockViewQuestion(Int32.Parse("0")));
        }
        public ActionResult Answer()
        {
            ViewBag.Message = "Post an Answer";

            if (Request["question_id"] != null)
            {
                return View(new  HomeModel.MockQuestion(Int32.Parse(Request["question_id"])));
            }
            else
            {
                Response.Redirect("~/");
                return null;
            }
        }

        public ActionResult Comment()
        {
            ViewBag.Message = "Post a Comment";

            if (Request["question_id"] != null)
            {
                return View(new TaxOverflow2013.Models.HomeModel.MockQuestion(Int32.Parse(Request["question_id"])));
            }
            else
            {
                Response.Redirect("~/");
                return null;
            }
        }
    }
}