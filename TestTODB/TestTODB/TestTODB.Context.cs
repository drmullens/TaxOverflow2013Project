﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestTODB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TestTODBContext : DbContext
    {
        public TestTODBContext()
            : base("name=TestTODBContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Questions> Questions1 { get; set; }
        public virtual DbSet<Answers> Answers1 { get; set; }
        public virtual DbSet<Categories> Categories1 { get; set; }
        public virtual DbSet<QuestionVotingHistory> QuestionVotingHistories { get; set; }
        public virtual DbSet<AnswerVotingHistory> AnswerVotingHistories { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<QuestionComment> QuestionComments { get; set; }
        public virtual DbSet<AnswerComment> AnswerComments { get; set; }
    }
}
