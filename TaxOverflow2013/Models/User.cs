//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxOverflow2013.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.AnswerComments = new HashSet<AnswerComment>();
            this.Answers1 = new HashSet<Answers1>();
            this.QuestionComments = new HashSet<QuestionComment>();
            this.Questions1 = new HashSet<Questions1>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int AnswerVotingHistory_AVH_ID { get; set; }
        public int QuestionVotingHistory_QVH_ID { get; set; }
    
        public virtual ICollection<AnswerComment> AnswerComments { get; set; }
        public virtual ICollection<Answers1> Answers1 { get; set; }
        public virtual AnswerVotingHistory AnswerVotingHistory { get; set; }
        public virtual ICollection<QuestionComment> QuestionComments { get; set; }
        public virtual ICollection<Questions1> Questions1 { get; set; }
        public virtual QuestionVotingHistory QuestionVotingHistory { get; set; }
    }
}