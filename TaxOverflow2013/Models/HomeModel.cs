using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                for (int i = 0; i < 5; i++)
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
                
                    for (int i = 0; i < 5; i++)
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
            public Random Votes = new Random();
            public int questionID = new int();
            public string referrer;
            public MockViewQuestion(int question_id, string _referrer) 
            {
                questionID = question_id;
                referrer = _referrer;

                LoadMockData();
            }
            public MockViewQuestion(int vote, char math)
            {
                LoadMockData();

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
    }
}