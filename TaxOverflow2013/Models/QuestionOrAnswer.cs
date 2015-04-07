using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxOverflow2013.Models
{
    public class QuestionOrAnswer
    {
        //Summary: this class is used for the comments page and will contain needed model data for both
        public QuestionList aQuestion { get; set; }

        public AnswerList anAnswer { get; set; }

        public CommentType type { get; set; }
    }

    public class AnswerList
    {
        public AnswerTBL anAnswer {get; set;}

        public string AUserName {get; set;}

    }

    public enum CommentType { Question, Answer}
}