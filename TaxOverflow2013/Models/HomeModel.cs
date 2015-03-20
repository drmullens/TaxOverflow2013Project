using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestTODB;

namespace TaxOverflow2013.Models
{
    public class HomeModel
    {
        public class MockIndexModel
        {
            public Random Votes = new Random();
            public List<string>[] Question = new List<string>[5];

            public MockIndexModel()
            {
                for (int i = 0; i < Question.Length; i++)
                {
                    Question[i] = new List<string>();
                }

                LoadMockData();
            }
                private void LoadMockData() 
                {
                    Question[0].Add("0");
                    Question[0].Add("Why is Java so much like coffee?");
                    Question[0].Add("200");
                    Question[0].Add("Technical");
                    Question[0].Add("Caroline Chonko");
                    Question[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                    Question[1].Add("1");
                    Question[1].Add("Why does no one on my team know Entity?");
                    Question[1].Add("100");
                    Question[1].Add("Technical");
                    Question[1].Add("David Mullens");
                    Question[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                    Question[2].Add("2");
                    Question[2].Add("What is the best programming language to use?");
                    Question[2].Add("50");
                    Question[2].Add("Technical");
                    Question[2].Add("Behrad Torkian");
                    Question[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                    Question[3].Add("3");
                    Question[3].Add("When is my project going to be done?");
                    Question[3].Add("25");
                    Question[3].Add("Technical");
                    Question[3].Add("Peter Mourfield");
                    Question[3].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                    Question[4].Add("4");
                    Question[4].Add("How much did each member contribute?");
                    Question[4].Add("18");
                    Question[4].Add("Technical");
                    Question[4].Add("Onyeka Ezenwoye");
                    Question[4].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                
                    for (int i = 0; i < Question.Length; i++)
                    {
                        if (Question[i][1].Length >= 40) {
                            Question[i][1] = Question[i][1].Substring(0, 40) + "...";
                        }
                    }
            }
        }

        public class MockViewQuestion
        {
            public List<string> Question = new List<string>();
            public List<string>[] QComment = new List<string>[3];
            public List<string>[] Answer = new List<string>[2];
            public List<string>[] AComment = new List<string>[1];

            public Random Votes = new Random();
            public int questionID = new int();
            public string referrer;
            public MockViewQuestion(int question_id) 
            {
                questionID = question_id;

                LoadMockQuestionData();
                LoadMockQCommentData();
                LoadMockAnswerData();
                LoadMockACommentData();
            }
            public MockViewQuestion(int question_id, int vote, char math)
            {
                questionID = question_id;

                LoadMockQuestionData();

                if (math == 'a') 
                {
                    Question[2] = Convert.ToString(vote + 1);
                }
                else if (math == 's')
                {
                    Question[2] = Convert.ToString(vote - 1);
                }
                else
                {
                    Question[2] = Convert.ToString(vote);
                }

                LoadMockQCommentData();
                LoadMockAnswerData();
                LoadMockACommentData();
            }
            private void LoadMockQuestionData() 
            {
                switch (questionID)
                {
                    case 0:
                        Question.Add("0");
                        Question.Add("Why is Java so much like coffee?");
                        Question.Add("200");
                        Question.Add("Technical");
                        Question.Add("Caroline Chonko");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 1:
                        Question.Add("1");
                        Question.Add("Why does no one on my team know Entity?");
                        Question.Add("100");
                        Question.Add("Technical");
                        Question.Add("David Mullens");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 2:
                        Question.Add("2");
                        Question.Add("What is the best programming language to use?");
                        Question.Add("50");
                        Question.Add("Technical");
                        Question.Add("Behrad Torkian");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 3:
                        Question.Add("3");
                        Question.Add("When is my project going to be done?");
                        Question.Add("25");
                        Question.Add("Technical");
                        Question.Add("Peter Mourfield");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 4:
                        Question.Add("4");
                        Question.Add("How much did each member contribute?");
                        Question.Add("18");
                        Question.Add("Technical");
                        Question.Add("Onyeka Ezenwoye");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    default:
                        break;
                }
            }
            private void LoadMockQCommentData()
            {
                for (int i = 0; i < QComment.Length; i++)
                {
                    QComment[i] = new List<string>();
                }
                //0 - QuestionID, 1 - CommentID, 2 - Comment, 3 - Author, 4 - TimeStamp

                switch (questionID)
                {
                    case 0:
                        QComment[0].Add("0");
                        QComment[0].Add("0");
                        QComment[0].Add("Other than the name, why do you think it is like coffee?");
                        QComment[0].Add("Clueless Wonder");
                        QComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[1].Add("0");
                        QComment[1].Add("1");
                        QComment[1].Add("Mostly it is the name...");
                        QComment[1].Add("Caroline Chonko");
                        QComment[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[2].Add("0");
                        QComment[2].Add("2");
                        QComment[2].Add("Oh...");
                        QComment[2].Add("Clueless Wonder");
                        QComment[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 1:
                        QComment[0].Add("1");
                        QComment[0].Add("3");
                        QComment[0].Add("Didn't get much use out of database class...Mostly learned MySQL.");
                        QComment[0].Add("Caroline Chonko");
                        QComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[1].Add("1");
                        QComment[1].Add("4");
                        QComment[1].Add("I didn't learn this in Database, I learned it on my own.");
                        QComment[1].Add("David Mullens");
                        QComment[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[2].Add("1");
                        QComment[2].Add("5");
                        QComment[2].Add("I have no good excuses at this time... Try again later.");
                        QComment[2].Add("Caroline Chonko");
                        QComment[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 2:
                        QComment[0].Add("2");
                        QComment[0].Add("6");
                        QComment[0].Add("So what is the Project you are working on exactly?");
                        QComment[0].Add("Caroline Chonko");
                        QComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[1].Add("2");
                        QComment[1].Add("7");
                        QComment[1].Add("I am building a Question and Answer Repository.");
                        QComment[1].Add("Behrad Torkian");
                        QComment[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[2].Add("2");
                        QComment[2].Add("8");
                        QComment[2].Add("Clearly you should use Java... Doesn't get much better than coffee. But if all else fails, use ASP.NET MVC5.");
                        QComment[2].Add("Caroline Chonko");
                        QComment[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 3:
                        QComment[0].Add("3");
                        QComment[0].Add("9");
                        QComment[0].Add("Apologies for the late updates, this is harder than it looks!");
                        QComment[0].Add("Caroline Chonko");
                        QComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[1].Add("3");
                        QComment[1].Add("10");
                        QComment[1].Add("Clearly you should have been done by now!");
                        QComment[1].Add("Snarky User");
                        QComment[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[2].Add("3");
                        QComment[2].Add("11");
                        QComment[2].Add("No hurries, I want it done right.");
                        QComment[2].Add("Peter Mourfield");
                        QComment[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 4:
                        QComment[0].Add("4");
                        QComment[0].Add("12");
                        QComment[0].Add("I find this a highly personal question that should be asked behind closed doors.");
                        QComment[0].Add("Caroline Chonko");
                        QComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[1].Add("4");
                        QComment[1].Add("13");
                        QComment[1].Add("I refuse to answer on grounds of self-incrimination. I plea the 5th.");
                        QComment[1].Add("Behrad Torkian");
                        QComment[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        QComment[2].Add("4");
                        QComment[2].Add("14");
                        QComment[2].Add("Just answer the question.");
                        QComment[2].Add("Onyeka Ezenwoye");
                        QComment[2].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    default:
                        break;
                }
            }
            private void LoadMockAnswerData()
            {
                //0 - QuestionID, 1 - AnswerID, 2 - Answer, 3 - Number of Votes, 4 - is Correct, 5 - Author, 6 - TimeStamp

                for (int i = 0; i < Answer.Length; i++)
                {
                    Answer[i] = new List<string>();
                }

                switch (questionID)
                {
                    case 0:
                        Answer[0].Add("0");
                        Answer[0].Add("0");
                        Answer[0].Add("Because clearly everyone loves coffee.");
                        Answer[0].Add(Votes.Next(100).ToString());
                        Answer[0].Add("false");
                        Answer[0].Add("Caroline");
                        Answer[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        Answer[1].Add("0");
                        Answer[1].Add("1");
                        Answer[1].Add("It came about because of a joke.");
                        Answer[1].Add(Votes.Next(100).ToString());
                        Answer[1].Add("true");
                        Answer[1].Add("Random User");
                        Answer[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 1:
                        Answer[0].Add("1");
                        Answer[0].Add("2");
                        Answer[0].Add("We are learning it slowly!");
                        Answer[0].Add(Votes.Next(100).ToString());
                        Answer[0].Add("false");
                        Answer[0].Add("Generic User");
                        Answer[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        Answer[1].Add("1");
                        Answer[1].Add("3");
                        Answer[1].Add("We are clearly not trying hard enough to learn it.");
                        Answer[1].Add(Votes.Next(100).ToString());
                        Answer[1].Add("true");
                        Answer[1].Add("Caroline Chonko");
                        Answer[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 2:
                        Answer[0].Add("2");
                        Answer[0].Add("4");
                        Answer[0].Add("Clearly Java because it is like Coffee.");
                        Answer[0].Add(Votes.Next(100).ToString());
                        Answer[0].Add("true");
                        Answer[0].Add("Random User");
                        Answer[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        Answer[1].Add("2");
                        Answer[1].Add("5");
                        Answer[1].Add("How about ASP.NET MVC?");
                        Answer[1].Add(Votes.Next(100).ToString());
                        Answer[1].Add("false");
                        Answer[1].Add("Generic User");
                        Answer[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 3:
                        Answer[0].Add("3");
                        Answer[0].Add("6");
                        Answer[0].Add("Never! Bwahahahah!");
                        Answer[0].Add(Votes.Next(100).ToString());
                        Answer[0].Add("false");
                        Answer[0].Add("Troll User");
                        Answer[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        Answer[1].Add("3");
                        Answer[1].Add("7");
                        Answer[1].Add("At the latest by May.");
                        Answer[1].Add(Votes.Next(100).ToString());
                        Answer[1].Add("true");
                        Answer[1].Add("Onyeka Ezenwoye");
                        Answer[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 4:
                        Answer[0].Add("4");
                        Answer[0].Add("8");
                        Answer[0].Add("Contribution is highly subjective and therefore should not make or break anyone's grades.");
                        Answer[0].Add(Votes.Next(100).ToString());
                        Answer[0].Add("false");
                        Answer[0].Add("Random User");
                        Answer[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

                        Answer[1].Add("4");
                        Answer[1].Add("9");
                        Answer[1].Add("I feel we all contributed enough to pass this class!");
                        Answer[1].Add(Votes.Next(100).ToString());
                        Answer[1].Add("true");
                        Answer[1].Add("Caroline");
                        Answer[1].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    default:
                        break;
                }

            }
            private void LoadMockACommentData()
            {
                for (int i = 0; i < AComment.Length; i++)
                {
                    AComment[i] = new List<string>();
                }
                        AComment[0].Add("");
                        AComment[0].Add("");
                        AComment[0].Add("This is a terrible Answer!");
                        AComment[0].Add("Snarky User");
                        AComment[0].Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
            }
        }

        public class MockQuestion
        {
            public List<string> Question = new List<string>();
            public Random Votes = new Random();
            public int questionID = new int();

            public MockQuestion(int question_id)
            {
                questionID = question_id;

                LoadMockData();
            }
            private void LoadMockData()
            {
                switch (questionID)
                {
                    case 0:
                        Question.Add("0");
                        Question.Add("Why is Java so much like coffee?");
                        Question.Add("200");
                        Question.Add("Technical");
                        Question.Add("Caroline Chonko");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;
                    case 1:
                        Question.Add("1");
                        Question.Add("Why does no one on my team know Entity?");
                        Question.Add("100");
                        Question.Add("Technical");
                        Question.Add("David Mullens");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 2:
                        Question.Add("2");
                        Question.Add("What is the best programming language to use?");
                        Question.Add("50");
                        Question.Add("Technical");
                        Question.Add("Behrad Torkian");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 3:
                        Question.Add("3");
                        Question.Add("When is my project going to be done?");
                        Question.Add("25");
                        Question.Add("Technical");
                        Question.Add("Peter Mourfield");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    case 4:
                        Question.Add("4");
                        Question.Add("How much did each member contribute?");
                        Question.Add("18");
                        Question.Add("Technical");
                        Question.Add("Onyeka Ezenwoye");
                        Question.Add(DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        break;

                    default:
                        break;
                }
            }
        }

        public class MockAnswer
        {
            

        }
    }

    public class AccessTODB
    {
        public void addNewQuestion()
        {
            using (var context = new TestTODBContext())
            {

            }
        }
    }

   
}