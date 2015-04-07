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

                foreach (var recent in MostRecent)
                {
                    QuestionList newQuestion = new QuestionList();
                    newQuestion.aQuestion = recent;
                    newQuestion.CategoryString = recent.CategoryTBL.Category;
                    newQuestion.UserName = recent.UserTBL.UserName;
                    HomePageQuestions.MostRecentQuestions.Add(newQuestion);

                }

                foreach (var highScores in BestScore)
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

                    //Return newest QuestionID
                    var QID = context.QuestionTBLs.Max(b => b.QuestionID);
                    myQuestion.QuestionID = QID;

                    NewQuestion.MainQuestion = myQuestion;
                    NewQuestion.QuestionUserName = User.Identity.Name;

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

            return View("ViewQuestion", NewQuestion);
        }

        public ActionResult QuestionList(int ddlSortBy = 0)  //[optional] ddlSortBy
        {
            ViewBag.Message = "View Questions";

            //<option value=0 >Select a Sort</option>
            //<option value=1 >Category: A - Z</option>
            //<option value=2 >Category: Z - A</option>
            //<option value=3 >Votes: High - Low</option>
            //<option value=4 >Votes: Low - High</option>
            //<option value=5 >Unanswered Questions</option>

            ShowQuestionLists QuestionList = new ShowQuestionLists();
            QuestionList.ShowQuestion = new List<QuestionListWithAnswered>();

            using (var context = new TODBEntities())
            {
                List<QuestionTBL> QList = new List<QuestionTBL>();
                switch (ddlSortBy)
                {

                    //create question list by drop down list selection default 0 by most recent date
                    case 1:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby (from C in context.CategoryTBLs orderby C.Category ascending select C)
                                     select R).ToList();
                            break;
                        }
                    case 2:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby (from C in context.CategoryTBLs orderby C.Category descending select C)
                                     select R).ToList();
                            break;
                        }
                    case 3:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby R.Score descending
                                     select R).ToList();
                            break;
                        }
                    case 4:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby R.Score ascending
                                     select R).ToList();
                            break;
                        }
                    case 5:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby (from A in context.AnswerTBLs orderby A.Accepted select A)
                                     select R).ToList();
                            break;
                        }
                    case 0:
                    default:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby R.QuestionDTS descending
                                     select R).ToList();
                            break;
                        }
                }

                foreach (var item in QList)
                {
                    QuestionListWithAnswered aQuestion = new QuestionListWithAnswered();
                    aQuestion.aQuestion = item;
                    var isAccepted = context.AnswerTBLs.Where(a => a.QuestionID == item.QuestionID).ToList();
                    if (isAccepted.Count > 0)
                    {
                        aQuestion.AccptedAnswer = true;
                    }
                    else
                    {
                        aQuestion.AccptedAnswer = false;
                    }
                    aQuestion.UserName = GetUserNameByID(item.UserID);
                    aQuestion.CategoryString = GetCategoryByID(item.CategoryID);
                    QuestionList.ShowQuestion.Add(aQuestion);
                }
            }

            return View(QuestionList);
        }

        public ActionResult ViewQuestion()
        {
            ViewBag.Message = "View a question";

            int QID = Int32.Parse(Request["question_id"]);

            QuestionStream CurrentQuestion = new QuestionStream();
            CurrentQuestion.RelatedQuestionComments = new List<QuestionCommentStream>();
            CurrentQuestion.RelatedAnswers = new List<AnswerStream>();
            
            QuestionTBL myQuestion = new QuestionTBL();

            using (var context = new TODBEntities())
            {
                var CurrQuestion = context.QuestionTBLs.Where(q => q.QuestionID == QID).ToList();
                if (CurrQuestion.Count > 0)
                {
                    myQuestion.QuestionID = QID;
                    foreach (var aQuestion in CurrQuestion)
                    {
                        myQuestion = aQuestion;
                    }

                    CurrentQuestion.MainQuestion = myQuestion;
                    CurrentQuestion.QuestionUserName = GetUserNameByID(myQuestion.UserID);
                    CurrentQuestion.QuestionCategory = GetCategoryByID(myQuestion.CategoryID);

                    var QComm = context.QuestionCommentTBLs.Where(a => a.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var comment in QComm)
                    {
                        QuestionCommentStream QComment = new QuestionCommentStream();
                        QComment.QComment = comment;
                        QComment.QCUserName = comment.UserTBL.UserName;
                        CurrentQuestion.RelatedQuestionComments.Add(QComment);
                    }

                    var QA = context.AnswerTBLs.Where(b => b.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var ans in QA)
                    {
                        AnswerStream anAnswer = new AnswerStream();
                        anAnswer.RelatedAnswerComments = new List<AnswerCommentStream>();

                        anAnswer.MainAnswer = ans;
                        anAnswer.AnswerUserName = GetUserNameByID(ans.UserID);
                        var ansComm = context.AnswerCommentTBLs.Where(d => d.AnswerID == ans.AnswerID).ToList();

                        foreach (var comment in ansComm)
                        {
                            AnswerCommentStream AComment = new AnswerCommentStream();
                            AComment.AComment = comment;
                            AComment.ACUserName = GetUserNameByID(comment.UserID);
                            anAnswer.RelatedAnswerComments.Add(AComment);
                        }

                        CurrentQuestion.RelatedAnswers.Add(anAnswer);
                    }

                }
                else
                {
                    Response.Redirect("/Error");
                    return (null);
                }
            }

            return View(CurrentQuestion);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ViewQuestion(string txtAnswer, string type = "Answer", string QuestionID = "0", string AnswerID = "0" )
        {
            int QID = 0;
            int AID = 0;

            if (type == "Answer")
                //This means a new Answer is being posted
            {
                if (Int32.TryParse(QuestionID, out QID))
                {
                    AnswerTBL newAnswer = new AnswerTBL();
                    newAnswer.UserID = GetCurrentUser();
                    newAnswer.Answer = txtAnswer;
                    newAnswer.QuestionID = QID;
                    newAnswer.Score = 0;
                    newAnswer.AnswerDTS = DateTime.Now;
                    newAnswer.Accepted = false;

                    QuestionStream showQuestion = new QuestionStream();
                    using (var context = new TODBEntities())
                    {
                        context.AnswerTBLs.Add(newAnswer);
                        context.SaveChanges();
                    }

                    showQuestion = getQuestionStream(QID);
                    if (showQuestion != null)
                    {
                        return View(showQuestion);
                    }
                }
            }
            else if (type == "QComment")
            {
                if (Int32.TryParse(QuestionID, out QID))
                {
                    QuestionCommentTBL newComment = new QuestionCommentTBL();
                    newComment.UserID = GetCurrentUser();
                    newComment.QComment = txtAnswer;
                    newComment.QuestionID = QID;
                    newComment.QCommentDTS = DateTime.Now;

                    QuestionStream showQuestion = new QuestionStream();
                    using (var context = new TODBEntities())
                    {
                        context.QuestionCommentTBLs.Add(newComment);
                        context.SaveChanges();
                    }

                    showQuestion = getQuestionStream(QID);
                    if (showQuestion != null)
                    {
                        return View(showQuestion);
                    }
                }
            }
            else if(type == "AComment")
            {
                if (Int32.TryParse(AnswerID, out AID))
                {
                    AnswerCommentTBL newComment = new AnswerCommentTBL();
                    newComment.UserID = GetCurrentUser();
                    newComment.AComment = txtAnswer;
                    newComment.AnswerID = AID;
                    newComment.AcommentDTS = DateTime.Now;

                    QuestionStream showQuestion = new QuestionStream();

                    using (var context = new TODBEntities())
                    {
                        context.AnswerCommentTBLs.Add(newComment);
                        context.SaveChanges();

                        var currentQuestion = context.AnswerTBLs.Where(c => c.AnswerID == AID).ToList();
                        if (currentQuestion.Count > 0)
                        {
                            foreach(var item in currentQuestion)
                            {
                                QID = item.QuestionID;
                            }

                            showQuestion = getQuestionStream(QID);
                            if (showQuestion != null)
                            {
                                return View(showQuestion);
                            }
                        }
                    }
                }
            }
            Response.Redirect("/Error");
            return null;
        }

        public ActionResult Search()
        {
            ViewBag.Message = "View Search Results";

            return View(new HomeModel.MockIndexModel());
        }

        public ActionResult Answer()
        {
            ViewBag.Message = "Post an Answer";

            int QID;

            QuestionList fullQuestion = new QuestionList();

            if (Int32.TryParse(Request["Question_id"], out QID))
            {
                using (var context = new TODBEntities())
                {
                    var currQuestion = context.QuestionTBLs.Where(c => c.QuestionID == QID).ToList();
                    if (currQuestion.Count > 0)
                    {
                        foreach (var QuestionInfo in currQuestion)
                        {
                            fullQuestion.aQuestion = QuestionInfo;
                            fullQuestion.CategoryString = GetCategoryByID(QuestionInfo.CategoryID);
                            fullQuestion.UserName = GetUserNameByID(QuestionInfo.UserID);
                        }
                    }

                }
                return View(fullQuestion);
            }

            Response.Redirect("~/");
            return null;
        }

        public ActionResult Comment()
        {
            ViewBag.Message = "Post a Comment";

            int ID;
            int AID;  //if there is a questionID and an answerID > 0, then this is an answer comment

            QuestionOrAnswer fullQuestion = new QuestionOrAnswer();
            if (Int32.TryParse(Request["answer_id"], out AID))
            {
                if (AID > 0)
                {
                    using (var context = new TODBEntities())
                    {
                        var currAnswer = context.AnswerTBLs.Where(c => c.AnswerID == AID).ToList();
                        foreach(var AnswerInfo in currAnswer)
                        {
                            fullQuestion.anAnswer = new AnswerList();
                            fullQuestion.anAnswer.anAnswer = AnswerInfo;
                            fullQuestion.anAnswer.AUserName = GetUserNameByID(AnswerInfo.UserID);
                            fullQuestion.type = CommentType.Answer;
                        }
                    }
                    return View(fullQuestion);
                }
            }
            if (Int32.TryParse(Request["question_id"], out ID))
            {
                using (var context = new TODBEntities())
                {
                    var currQuestion = context.QuestionTBLs.Where(c => c.QuestionID == ID).ToList();
                    if (currQuestion.Count > 0)
                    {
                        foreach (var QuestionInfo in currQuestion)
                        {
                            fullQuestion.aQuestion = new QuestionList();
                            fullQuestion.aQuestion.aQuestion = QuestionInfo;
                            fullQuestion.aQuestion.CategoryString = GetCategoryByID(QuestionInfo.CategoryID);
                            fullQuestion.aQuestion.UserName = GetUserNameByID(QuestionInfo.UserID);
                            fullQuestion.type = CommentType.Question;
                        }
                    }

                }
                return View(fullQuestion);
            }

            Response.Redirect("~/");
            return null;
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

        private string GetUserNameByID(int id)
        {
            using (var context = new TODBEntities())
            {
                var idStrings = context.UserTBLs.Where(c => c.UserID == id).ToList();
                if (idStrings.Count > 0)
                {
                    foreach (var userName in idStrings)
                    {
                        return userName.UserName;
                    }
                }
            }

            return "";  //No such value, redirect to error screen
        }

        private string GetCategoryByID(int catID)
        {
            using (var context = new TODBEntities())
            {
                var catString = context.CategoryTBLs.Where(c => c.CategoryID == catID).ToList();
                if (catString.Count > 0)
                {
                    foreach (var catItem in catString)
                    {
                        return catItem.Category;
                    }
                }
            }

            return "";  //No such value, redirect to error screen
        }

        private QuestionStream getQuestionStream(int QID)
        {
            QuestionStream aQuestionStream = new QuestionStream();
            aQuestionStream.RelatedAnswers = new List<AnswerStream>();
            aQuestionStream.RelatedQuestionComments = new List<QuestionCommentStream>();

            using (var context = new TODBEntities())
            {
                var QStream = context.QuestionTBLs.Where(c => c.QuestionID == QID).ToList();
                if (QStream.Count > 0)
                {
                    foreach (var question in QStream)
                    {
                        aQuestionStream.MainQuestion = question;
                        aQuestionStream.QuestionUserName = GetUserNameByID(question.UserID);
                        aQuestionStream.QuestionCategory = GetCategoryByID(question.CategoryID);
                    }

                    var QComm = context.QuestionCommentTBLs.Where(a => a.QuestionID == aQuestionStream.MainQuestion.QuestionID).ToList();
                    foreach (var comment in QComm)
                    {
                        QuestionCommentStream QComment = new QuestionCommentStream();
                        QComment.QComment = comment;
                        QComment.QCUserName = comment.UserTBL.UserName;
                        aQuestionStream.RelatedQuestionComments.Add(QComment);
                    }

                    var QA = context.AnswerTBLs.Where(b => b.QuestionID == aQuestionStream.MainQuestion.QuestionID).ToList();
                    foreach (var ans in QA)
                    {
                        AnswerStream anAnswer = new AnswerStream();
                        anAnswer.RelatedAnswerComments = new List<AnswerCommentStream>();

                        anAnswer.MainAnswer = ans;
                        var ansComm = context.AnswerCommentTBLs.Where(d => d.AnswerID == ans.AnswerID).ToList();
                        foreach (var comment in ansComm)
                        {
                            AnswerCommentStream AComment = new AnswerCommentStream();
                            AComment.AComment = comment;
                            AComment.ACUserName = comment.UserTBL.UserName;
                            anAnswer.RelatedAnswerComments.Add(AComment);
                        }

                        aQuestionStream.RelatedAnswers.Add(anAnswer);
                    }
                }
            }
            return aQuestionStream;
        }

        #endregion
    }
}