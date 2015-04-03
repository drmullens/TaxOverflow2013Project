using System;
using System.Web.Mvc;
using TaxOverflow2013.Models;
using System.Linq;

namespace TaxOverflow2013.Controllers
{
    public class HomeController : Controller
    {
        TODBEntities context = new TODBEntities();
        CurrentData currData = new CurrentData();

        public ActionResult Index()
        {
            string currentUser;

            currentUser = User.Identity.Name;

            using (context)
            {
                var users = context.UserTBLs.Where(c => c.UserName == currentUser).ToList();
                if (users.Count() == 0)
                {
                    UserTBL newUser = new UserTBL();

                    newUser.UserName = User.Identity.Name;
                    
                    context.UserTBLs.Add(newUser);
                    context.SaveChanges();

                    users = context.UserTBLs.Where(c => c.UserName == currentUser).ToList();
                }
               
                foreach (var person in users)
                {
                    currData.CurrentUser = person.UserName;
                    currData.CurrentUserID = person.UserID;
                }
            }

            //TODO: Set up home page to get the top five by highest score and most recent
            return View(new HomeModel.MockIndexModel());
        }

        public ActionResult Question()
        {
            ViewBag.Message = "Post a Question";
            // TODO: load the DDL with the categories from the DB

            using (context)
            {
                var categories = context.CategoryTBLs.Where(c => c.CategoryID > 0).ToList();
                return View(categories);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Question(string txtQuestion, string ddlCategory) //[optional] string newCategory = ""
        {
            string data = txtQuestion;

            //Return new QuestionID

            try
            {
                QuestionTBL myQuestion = new QuestionTBL();

                myQuestion.Question = txtQuestion;
                myQuestion.Score = 0;
                myQuestion.QuestionDTS = DateTime.Now;
                myQuestion.UserID = currData.CurrentUserID;

                int ddlValue;
                if (Int32.TryParse(ddlCategory, out ddlValue))
                    myQuestion.CategoryID = ddlValue;
                else
                    //TODO: logic here to handle problem with a non-int value being passed as ddlCategory
                    ddlValue = 0;
                using (context)
                {

                    context.QuestionTBLs.Add(myQuestion);
                    //context.SaveChanges();

                    var QID = context.QuestionTBLs.Max(b=> b.QuestionID);
                    myQuestion.QuestionID = QID;
                    ViewData.Add("QuestionID", myQuestion.QuestionID);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} in post question", ex);
            }
            

            //TODO: dont send the whole db, create a model that would be the question, list of comments, answers and their comments
            return View("ViewQuestion", new TODBEntities());
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

        public ActionResult Answer()
        {
            ViewBag.Message = "Post an Answer";

            if (Request["question_id"] != null)
            {
                return View(new HomeModel.MockQuestion(Int32.Parse(Request["question_id"])));
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