//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxOverflow.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnswerCommentTBL
    {
        public int ACommentID { get; set; }
        public string AComment { get; set; }
        public System.DateTime AcommentDTS { get; set; }
        public int AnswerID { get; set; }
        public int UserID { get; set; }
    
        public virtual AnswerTBL AnswerTBL { get; set; }
        public virtual UserTBL UserTBL { get; set; }
    }
}
