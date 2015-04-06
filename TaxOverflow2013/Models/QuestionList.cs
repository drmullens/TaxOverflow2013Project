using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxOverflow2013.Models
{
    public class QuestionList
    {

        public QuestionTBL aQuestion { get; set; }

        public string CategoryString { get; set; }

        public string UserName { get; set; }
    }

    //this model is used on the home page for the top 5 most recent and top 5 unanswered
    public class HomeQuestionLists
    {
        public List<QuestionList> MostRecentQuestions { get; set; }

        public List<QuestionList> BestUnansweredQuestions { get; set; }
    }
}