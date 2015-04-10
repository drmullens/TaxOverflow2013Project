using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxOverflow2013.Models
{

    //Summary: these classes are for returning a model of a single question.  All comments, answers and their comments will be in this model
    //a change for a push

    public class QuestionStream
    {
        public QuestionTBL MainQuestion { get; set; }

        public string QuestionUserName { get; set; }

        public string QuestionCategory { get; set; }

        public bool AcceptedAnswer { get; set; }

        public int CurrentUserID { get; set; }

        public List<AnswerStream> RelatedAnswers { get; set; }

        public List<QuestionCommentStream> RelatedQuestionComments { get; set; }
    }

    public class AnswerStream
    {
        public AnswerTBL MainAnswer { get; set; }

        public string AnswerUserName { get; set; }

        public List<AnswerCommentStream> RelatedAnswerComments { get; set; }
    }

    public class QuestionCommentStream
    {
        public QuestionCommentTBL QComment { get; set; }

        public string QCUserName { get; set; }

    }

    public class AnswerCommentStream
    {
        public AnswerCommentTBL AComment { get; set; }

        public string ACUserName { get; set; }
    }
}