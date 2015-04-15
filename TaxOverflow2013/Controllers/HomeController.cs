using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaxOverflow2013.Models;

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
                    newUser.Reputation = 0;

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
                    newQuestion.aQuestion.Question = System.Text.RegularExpressions.Regex.Replace(newQuestion.aQuestion.Question,
                        "<[^>]*(>|$)", "");
                    HomePageQuestions.MostRecentQuestions.Add(newQuestion);

                }

                foreach (var highScores in BestScore)
                {
                    QuestionList newQuestion = new QuestionList();
                    newQuestion.aQuestion = highScores;
                    newQuestion.CategoryString = highScores.CategoryTBL.Category;
                    newQuestion.UserName = highScores.UserTBL.UserName;
                    newQuestion.aQuestion.Question = System.Text.RegularExpressions.Regex.Replace(newQuestion.aQuestion.Question,
                        "<[^>]*(>|$)", "");
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
                var categories = context.CategoryTBLs.Where(c => c.CategoryID > 0).OrderBy(c => c.Category).ToList();
                return View(categories);
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Question(string txtQuestion, string ddlCategory, string txtOther = "")
        {
            if (string.IsNullOrWhiteSpace(txtQuestion))
            {
                Index();
                return View("Index");
            }

            string data = txtQuestion;
            QuestionStream NewQuestion = new QuestionStream();

            if (ddlCategory.ToUpper() == "OTHER" && txtOther != "" && CheckForDup(txtOther))
            {
                ddlCategory = AddNewCategory(txtOther);
                NewQuestion.QuestionCategory = txtOther;
            }

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
                        NewQuestion.QReputation = comment.UserTBL.Reputation;
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
                                     orderby R.CategoryTBL.Category ascending
                                     select R).ToList();
                            break;
                        }
                    case 2:
                        {
                            QList = (from R in context.QuestionTBLs
                                     orderby R.CategoryTBL.Category descending
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
                                     let Accepted = (from A in context.AnswerTBLs where !A.Accepted select A.QuestionID)
                                     where Accepted.Contains(R.QuestionID) || R.AnswerTBLs.Count == 0
                                     select R).ToList();
                            break;
                        }
                    case 6:
                        {
                            QList = (from R in context.QuestionTBLs
                                     let Accepted = (from A in context.AnswerTBLs where A.Accepted select A.QuestionID)
                                     where Accepted.Contains(R.QuestionID)
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
                    aQuestion.aQuestion.UserTBL.Reputation = item.UserTBL.Reputation;
                }
            }

            return View(QuestionList);
        }

        public ActionResult ViewQuestion(string question_id = "", string answer_id = "", string vote = "")
        {
            ViewBag.Message = "View a question";

            int QID = 0;
            int AID = 0;
            int voteNum;

            if (question_id == "")
            {
                if (!Int32.TryParse(Request["question_id"], out QID))
                {
                    Index();
                    return View("Index");
                }
            }
            else
            {
                if (!Int32.TryParse(question_id, out QID))
                {
                    Index();
                    return View("Index");
                }
            }

            if (answer_id != "")
            {
                if (!Int32.TryParse(answer_id, out AID))
                {
                    Index();
                    return View("Index");
                }
            }

            if (vote != "")
            {
                //vote up or down on a question or an answer
                voteNum = Int32.Parse(vote);
                UpdateReputation(QID, AID, voteNum);
            }
            else if (AID != 0 && QID != 0)
            {
                //if both are != 0 then this is marking a question as accepted
                int UserID = GetCurrentUser();
                UpdateAcceptedAnswer(QID, AID, UserID);
            }

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
                        CurrentQuestion.QReputation = aQuestion.UserTBL.Reputation;
                    }

                    CurrentQuestion.MainQuestion = myQuestion;
                    CurrentQuestion.CurrentUserID = GetCurrentUser();
                    CurrentQuestion.QuestionUserName = GetUserNameByID(myQuestion.UserID);
                    CurrentQuestion.QuestionCategory = GetCategoryByID(myQuestion.CategoryID);

                    var QComm = context.QuestionCommentTBLs.Where(a => a.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var comment in QComm)
                    {
                        QuestionCommentStream QComment = new QuestionCommentStream();
                        QComment.QComment = comment;
                        QComment.QCReputation = comment.UserTBL.Reputation;
                        QComment.QCUserName = comment.UserTBL.UserName;
                        CurrentQuestion.RelatedQuestionComments.Add(QComment);
                    }

                    var QA = context.AnswerTBLs.Where(b => b.QuestionID == myQuestion.QuestionID).ToList();
                    foreach (var ans in QA)
                    {
                        AnswerStream anAnswer = new AnswerStream();
                        anAnswer.RelatedAnswerComments = new List<AnswerCommentStream>();
                        anAnswer.AReputation = ans.UserTBL.Reputation;
                        anAnswer.MainAnswer = ans;
                        anAnswer.AnswerUserName = GetUserNameByID(ans.UserID);
                        var ansComm = context.AnswerCommentTBLs.Where(d => d.AnswerID == ans.AnswerID).ToList();

                        foreach (var comment in ansComm)
                        {
                            AnswerCommentStream AComment = new AnswerCommentStream();
                            AComment.AComment = comment;
                            AComment.ACReputation = comment.UserTBL.Reputation;
                            AComment.ACUserName = GetUserNameByID(comment.UserID);
                            anAnswer.RelatedAnswerComments.Add(AComment);
                        }

                        CurrentQuestion.RelatedAnswers.Add(anAnswer);
                    }

                }
                else
                {
                    Index();
                    return View("Index");
                }
            }

            return View(CurrentQuestion);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ViewQuestion(string txtAnswer, string type = "Answer", string QuestionID = "0", string AnswerID = "0")
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
            else if (type == "AComment")
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
                            foreach (var item in currentQuestion)
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
            Index();
            return View("Index");
        }

        [HttpPost]
        public ActionResult Search(string UserNameSearch)
        {
            ViewBag.Message = "View Search Results";

            int userID = GetUserIdByName(UserNameSearch);

            if (string.IsNullOrWhiteSpace(UserNameSearch) && userID == 0)
            {
                Index();
                return View("Index");
            }

            SearchQuestionLists aQuestionList = new SearchQuestionLists();
            aQuestionList.ShowQuestion = new List<QuestionListWithAnswered>();

            List<QuestionTBL> QList = new List<QuestionTBL>();

            using (var context = new TODBEntities())
            {
                QList = (from R in context.QuestionTBLs
                         where (R.UserID == userID)
                         orderby R.QuestionDTS descending
                         select R).ToList();

                aQuestionList.ResultCount = QList.Count;

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
                    aQuestionList.ShowQuestion.Add(aQuestion);
                    aQuestion.aQuestion.UserTBL.Reputation = item.UserTBL.Reputation;
                }
            }


            return View(aQuestionList);
        }

        public ActionResult Answer(string question_id = "")
        {
            ViewBag.Message = "Post an Answer";

            int QID = 0;

            if ((!Int32.TryParse(Request["question_id"], out QID)) && (!Int32.TryParse(question_id, out QID)))
            {
                Index();
                return View("Index");
            }


            QuestionList fullQuestion = new QuestionList();

            if (QID != 0)
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
                            fullQuestion.aQuestion.UserTBL.Reputation = QuestionInfo.UserTBL.Reputation;
                        }
                    }

                }
                return View(fullQuestion);
            }

            Index();
            return View("Index");
        }

        public ActionResult Comment(string question_id)
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
                        foreach (var AnswerInfo in currAnswer)
                        {
                            fullQuestion.anAnswer = new AnswerList();
                            fullQuestion.anAnswer.anAnswer = AnswerInfo;
                            fullQuestion.anAnswer.AUserName = GetUserNameByID(AnswerInfo.UserID);
                            fullQuestion.type = CommentType.Answer;
                            fullQuestion.anAnswer.anAnswer.UserTBL.Reputation = AnswerInfo.UserTBL.Reputation;

                        }
                    }
                    return View(fullQuestion);
                }
            }
            if (Int32.TryParse(Request["question_id"], out ID) || Int32.TryParse(question_id, out ID))
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
                            fullQuestion.aQuestion.aQuestion.UserTBL.Reputation = QuestionInfo.UserTBL.Reputation;
                        }
                    }

                }
                return View(fullQuestion);
            }

            Index();
            return View("Index");
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
            aQuestionStream.AcceptedAnswer = false;

            using (var context = new TODBEntities())
            {
                var QStream = context.QuestionTBLs.Where(c => c.QuestionID == QID).ToList();
                if (QStream.Count > 0)
                {
                    foreach (var question in QStream)
                    {
                        aQuestionStream.MainQuestion = question;
                        aQuestionStream.QReputation = question.UserTBL.Reputation;
                        aQuestionStream.CurrentUserID = GetCurrentUser();
                        aQuestionStream.QuestionUserName = GetUserNameByID(question.UserID);
                        aQuestionStream.QuestionCategory = GetCategoryByID(question.CategoryID);
                    }

                    var QComm = context.QuestionCommentTBLs.Where(a => a.QuestionID == aQuestionStream.MainQuestion.QuestionID).ToList();
                    foreach (var comment in QComm)
                    {
                        QuestionCommentStream QComment = new QuestionCommentStream();
                        QComment.QComment = comment;
                        QComment.QCReputation = comment.UserTBL.Reputation;
                        QComment.QCUserName = comment.UserTBL.UserName;
                        aQuestionStream.RelatedQuestionComments.Add(QComment);
                    }

                    var QA = context.AnswerTBLs.Where(b => b.QuestionID == aQuestionStream.MainQuestion.QuestionID).ToList();
                    foreach (var ans in QA)
                    {
                        AnswerStream anAnswer = new AnswerStream();
                        anAnswer.RelatedAnswerComments = new List<AnswerCommentStream>();
                        anAnswer.AReputation = ans.UserTBL.Reputation;
                        anAnswer.MainAnswer = ans;
                        if (ans.Accepted == true)
                        {
                            aQuestionStream.AcceptedAnswer = true;
                        }
                        var ansComm = context.AnswerCommentTBLs.Where(d => d.AnswerID == ans.AnswerID).ToList();
                        foreach (var comment in ansComm)
                        {
                            AnswerCommentStream AComment = new AnswerCommentStream();
                            AComment.AComment = comment;
                            AComment.ACReputation = comment.UserTBL.Reputation;
                            AComment.ACUserName = comment.UserTBL.UserName;
                            anAnswer.RelatedAnswerComments.Add(AComment);
                        }

                        aQuestionStream.RelatedAnswers.Add(anAnswer);
                    }
                }
            }
            return aQuestionStream;
        }

        private int GetUserIdByName(string userName)
        {
            int userID = 0;
            using (var context = new TODBEntities())
            {
                var aUser = context.UserTBLs.Where(a => a.UserName.Contains(userName)).ToList();
                if (aUser.Count == 0)
                {
                    return userID;
                }

                else
                {
                    foreach (var searchUser in aUser)
                    {
                        userID = searchUser.UserID;
                    }
                }
            }
            return userID;
        }

        #endregion

        private void UpdateReputation(int QID, int AID, int type)
        {
            int currUser = GetCurrentUser();
            //Having an answer accepted: 15
            //accepting an answer: 2
            //having an answer voted up: 10
            //have a question voted up: 5
            //having a question voted down: -2
            //voting an answer or question down: -1
            using (var context = new TODBEntities())
            {

                switch (type)
                {
                    case 1:
                        {
                            //Question voted up
                            var checkvote = context.QuestionVotingHistoryTBLs.Where(a => a.QVHQuestionID == QID && a.QVHUserID == currUser).ToList();
                            if (checkvote.Count == 0)
                            {
                                var myQuestion = context.QuestionTBLs.Where(c => c.QuestionID == QID).ToList();
                                foreach (var aQuestion in myQuestion)
                                {
                                    aQuestion.Score += 1;

                                    var AskerID = context.UserTBLs.Where(b => b.UserID == aQuestion.UserID).ToList();
                                    foreach (var aUser in AskerID)
                                    {
                                        aUser.Reputation += 5;

                                        QuestionVotingHistoryTBL QHistory = new QuestionVotingHistoryTBL();

                                        QHistory.QVHQuestionID = aQuestion.QuestionID;
                                        QHistory.QVHUserID = currUser;
                                        QHistory.QVHDTS = DateTime.Now;

                                        context.QuestionVotingHistoryTBLs.Add(QHistory);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            //Question voted down
                            var checkvote = context.QuestionVotingHistoryTBLs.Where(a => a.QVHQuestionID == QID && a.QVHUserID == currUser).ToList();
                            if (checkvote.Count == 0)
                            {
                                var myQuestion = context.QuestionTBLs.Where(c => c.QuestionID == QID).ToList();
                                foreach (var aQuestion in myQuestion)
                                {
                                    aQuestion.Score -= 1;

                                    var AskerID = context.UserTBLs.Where(b => b.UserID == aQuestion.UserID).ToList();
                                    foreach (var aUser in AskerID)
                                    {
                                        aUser.Reputation -= 2;

                                        var raterID = context.UserTBLs.Where(d => d.UserID == currUser).ToList();
                                        foreach (var rater in raterID)
                                        {
                                            rater.Reputation -= 1;

                                            QuestionVotingHistoryTBL QHistory = new QuestionVotingHistoryTBL();

                                            QHistory.QVHQuestionID = aQuestion.QuestionID;
                                            QHistory.QVHUserID = currUser;
                                            QHistory.QVHDTS = DateTime.Now;

                                            context.QuestionVotingHistoryTBLs.Add(QHistory);
                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            //Answer voted up
                            var checkvote = context.AnswerVotingHistoryTBLs.Where(a => a.AVHAnswerID == QID && a.AVHUserID == currUser).ToList();
                            if (checkvote.Count == 0)
                            {
                                var myAnswer = context.AnswerTBLs.Where(b => b.AnswerID == AID).ToList();
                                foreach (var anAnswer in myAnswer)
                                {
                                    anAnswer.Score += 1;

                                    var AnswererID = context.UserTBLs.Where(c => c.UserID == anAnswer.UserID).ToList();
                                    foreach (var aUser in AnswererID)
                                    {
                                        aUser.Reputation += 10;

                                        AnswerVotingHistoryTBL AHistory = new AnswerVotingHistoryTBL();

                                        AHistory.AVHAnswerID = anAnswer.AnswerID;
                                        AHistory.AVHUserID = currUser;
                                        AHistory.AVHDTS = DateTime.Now;

                                        context.AnswerVotingHistoryTBLs.Add(AHistory);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            //Answer voted down

                            var checkvote = context.AnswerVotingHistoryTBLs.Where(a => a.AVHAnswerID == QID && a.AVHUserID == currUser).ToList();
                            if (checkvote.Count == 0)
                            {
                                var myAnswer = context.AnswerTBLs.Where(b => b.AnswerID == AID).ToList();
                                foreach (var anAnswer in myAnswer)
                                {
                                    anAnswer.Score -= 1;

                                    var AnswererID = context.UserTBLs.Where(c => c.UserID == anAnswer.UserID).ToList();
                                    foreach (var aUser in AnswererID)
                                    {
                                        aUser.Reputation -= 2;

                                        var raterID = context.UserTBLs.Where(d => d.UserID == currUser).ToList();
                                        foreach (var rater in raterID)
                                        {
                                            rater.Reputation -= 1;

                                            AnswerVotingHistoryTBL AHistory = new AnswerVotingHistoryTBL();

                                            AHistory.AVHAnswerID = anAnswer.AnswerID;
                                            AHistory.AVHUserID = currUser;
                                            AHistory.AVHDTS = DateTime.Now;

                                            context.AnswerVotingHistoryTBLs.Add(AHistory);
                                            context.SaveChanges();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {
                            //Some type of error, send to error page
                            break;
                        }
                }
            }

        }

        private void UpdateAcceptedAnswer(int QuestionID, int AnswerID, int UserID)
        {
            using (var context = new TODBEntities())
            {
                var Accepted = context.AnswerTBLs.Where(a => a.AnswerID == AnswerID).ToList();
                foreach (AnswerTBL anAnswer in Accepted)
                {
                    anAnswer.Accepted = true;
                    anAnswer.UserTBL.Reputation += 15;
                }
                context.SaveChanges();
            }
        }

        private string AddNewCategory(string newCategory)
        {
            using (var context = new TODBEntities())
            {
                CategoryTBL newCat = new CategoryTBL();

                newCat.Category = newCategory;

                context.CategoryTBLs.Add(newCat);

                context.SaveChanges();

                string catID = context.CategoryTBLs.Max(a => a.CategoryID).ToString();

                return catID;
            }
        }

        private bool CheckForDup(string category)
        {
            using (var context = new TODBEntities())
            {
                var dup = context.CategoryTBLs.Where(a => a.Category == category).ToList();
                if (dup.Count > 0)
                    return false;
            }
            return true;
        }

    }
}