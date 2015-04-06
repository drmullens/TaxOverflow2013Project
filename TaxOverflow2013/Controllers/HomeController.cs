using System;
using System.Web.Mvc;
using TaxOverflow2013.Models;
using System.Linq;
using System.Collections.Generic;

namespace TaxOverflow2013.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index() 
        {
            string currentUser;

            currentUser = User.Identity.Name;

            //HomePageQuestions contains 2 lists, the top 5 most recent and top 5 highest scores without an accepted answer
            HomeQuestionLists HomePageQuestions = new HomeQuestionLists();
            HomePageQuestions.MostRecentQuestions = new List<QuestionList>();
            HomePageQuestions.BestUnansweredQuestions = new List<QuestionList>();
            
            using (var context = new TODBEntities())
            {
                var users = context.UserTBLs.Where(c => c.UserName == currentUser).ToList();

                //add new userName to UserTBL
                if (users.Count() == 0)
                {
                    UserTBL newUser = new UserTBL();

                    newUser.UserName = User.Identity.Name;

                    context.UserTBLs.Add(newUser);
                    context.SaveChanges();

                }


                //get the top 5 most recent and top 5 highest scores without an accepted answer
                var MostRecent = (from R in context.QuestionTBLs
                                  orderby R.QuestionDTS descending
                                  where !(from A in context.AnswerTBLs select A.QuestionID).Contains(R.QuestionID)
                                  select R).Take(5).ToList();

                var BestScore = (from S in context.QuestionTBLs
                                 orderby S.Score descending
                                 where !(from A in context.AnswerTBLs where A.Accepted == true select A.QuestionID).Contains(S.QuestionID)
                                 select S).Take(5).ToList();


                //fill model lists with information
                
                foreach(var recent in MostRecent)
                {
                    QuestionList newQuestion = new QuestionList();
                    newQuestion.aQuestion = recent;
                    newQuestion.CategoryString = recent.CategoryTBL.Category;
                    newQuestion.UserName = recent.UserTBL.UserName;
                    HomePageQuestions.MostRecentQuestions.Add(newQuestion);

                }

                foreach(var highScores in BestScore)
                {
                    QuestionList newQuestion = new QuestionList();
                    newQuestion.aQuestion = highScores;
                    newQuestion.CategoryString = highScores.CategoryTBL.Category;
                    newQuestion.UserName = highScores.UserTBL.UserName;
                    HomePageQuestions.BestUnansweredQuestions.Add(newQuestion);
                }
            }

            //TODO: Set up home page to get the top five by highest score and most recent
            return View(HomePageQuestions);
        }

        public ActionResult Question()
        {
            ViewBag.Message = "Post a Question";
            TODBEntities context = new TODBEntities();
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
            QuestionStream NewQuestion = new QuestionStream();

            try
            {
                QuestionTBL myQuestion = new QuestionTBL();

                myQuestion.Question = txtQuestion;
                myQuestion.Score = 0;
                myQuestion.QuestionDTS = DateTime.Now;
                myQuestion.UserID = GetCurrentUser();


                int ddlValue;
                if (Int32.TryParse(ddlCategory, out ddlValue))
                    myQuestion.CategoryID = ddlValue;
                else
                    //TODO: logic here to handle problem with a non-int value being passed as ddlCategory
                    ddlValue = 0;
                using (var context = new TODBEntities())
                {
                    //adds new record of question to the DB
                    context.QuestionTBLs.Add(myQuestion);
                    context.SaveChanges();

                    //Return new QuestionID
                    var QID = context.QuestionTBLs.Max(b => b.QuestionID);  
                    myQuestion.QuestionID = QID;

                    NewQuestion.MainQuestion = myQuestion;

                    var QComm = context.QuestionCommentTBLs.Where(a => a.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var comment in QComm)
                    {
                        QuestionCommentStream QComment = new QuestionCommentStream();
                        QComment.QComment = comment;
                        QComment.QCUserName = comment.UserTBL.UserName;
                        NewQuestion.RelatedQuestionComments.Add(QComment);
                    }

                    var QA = context.AnswerTBLs.Where(b => b.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var ans in QA)
                    {
                        AnswerStream anAnswer = new AnswerStream();
                        anAnswer.MainAnswer = ans;
                        var ansComm = context.AnswerCommentTBLs.Where(d => d.AnswerID == ans.AnswerID).ToList();
                        foreach (var comment in ansComm)
                        {
                            AnswerCommentStream AComment = new AnswerCommentStream();
                            AComment.AComment = comment;
                            AComment.ACUserName = comment.UserTBL.UserName;
                            anAnswer.RelatedAnswerComments.Add(AComment);
                        }

                        NewQuestion.RelatedAnswers.Add(anAnswer);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} in post question", ex);
            }


            //TODO: dont send the whole db, create a model that would be the question, list of comments, answers and their comments
            return View("ViewQuestion", NewQuestion);
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


        #region "Get functions"

        private int GetCurrentUser()
        {
                using (var context = new TODBEntities())
                {
                    var currUser = context.UserTBLs.Where(c => c.UserName == User.Identity.Name).ToList();
                    if (currUser.Count > 0)
                    {
                        foreach (var person in currUser)
                        {
                            return person.UserID;
                        }
                    }
                }
                return -1;  //no such value send -1 to show error
        }

        private string GetCategoryByID(int catID)
        {
            using (var context = new TODBEntities())
            {
                var catString = context.CategoryTBLs.Where(c => c.CategoryID == catID).ToList();
                if (catString.Count > 0 )
                {
                    foreach(var catItem in catString)
                    {
                        return catItem.Category;
                    }
                }
            }

            return "";  //No such value, redirect to error screen
        }

        #endregion
    }
}