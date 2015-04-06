
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/08/2015 19:57:30
-- Generated from EDMX file: c:\users\david\documents\visual studio 2013\Projects\TestTODB\TestTODB\TestTODB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\DAVID\DOCUMENTS\TESTTODB.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Questions1'
CREATE TABLE [dbo].[Questions1] (
    [QuestionID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Question] nvarchar(max)  NOT NULL,
    [Score] int  NOT NULL,
    [QuestionDTS] datetime DEFAULT GETDATE() NOT NULL,
    [UserID] int  NOT NULL,
    [Category_CategoryID] int  NOT NULL
);
GO

-- Creating table 'Answers1'
CREATE TABLE [dbo].[Answers1] (
    [AnswerID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Answer] nvarchar(max)  NOT NULL,
    [Score] int  NOT NULL,
    [AnswerDTS] datetime DEFAULT GETDATE() NOT NULL,
    [Answered] bit  NOT NULL,
    [QuestionID] int  NOT NULL,
    [UserID] int  NOT NULL
);
GO

-- Creating table 'Categories1'
CREATE TABLE [dbo].[Categories1] (
    [CategoryID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Category] nvarchar(15)  NOT NULL
);
GO

-- Creating table 'QuestionVotingHistories'
CREATE TABLE [dbo].[QuestionVotingHistories] (
    [QVH_ID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Question_QuestionID] int  NOT NULL
);
GO

-- Creating table 'AnswerVotingHistories'
CREATE TABLE [dbo].[AnswerVotingHistories] (
    [AVH_ID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Answer_AnswerID] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [AnswerVotingHistory_AVH_ID] int  NOT NULL,
    [QuestionVotingHistory_QVH_ID] int  NOT NULL
);
GO

-- Creating table 'QuestionComments'
CREATE TABLE [dbo].[QuestionComments] (
    [QCommentID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [QCommentDTS] datetime DEFAULT GETDATE()  NOT NULL,
    [QuestionID] int  NOT NULL,
    [UserID] int  NOT NULL
);
GO

-- Creating table 'AnswerComments'
CREATE TABLE [dbo].[AnswerComments] (
    [ACommentID] int IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [ACommentDTS] datetime DEFAULT GETDATE() NOT NULL,
    [AnswerID] int  NOT NULL,
    [UserID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [QuestionID] in table 'Questions1'
ALTER TABLE [dbo].[Questions1]
ADD CONSTRAINT [PK_Questions1]
    PRIMARY KEY CLUSTERED ([QuestionID] ASC);
GO

-- Creating primary key on [AnswerID] in table 'Answers1'
ALTER TABLE [dbo].[Answers1]
ADD CONSTRAINT [PK_Answers1]
    PRIMARY KEY CLUSTERED ([AnswerID] ASC);
GO

-- Creating primary key on [CategoryID] in table 'Categories1'
ALTER TABLE [dbo].[Categories1]
ADD CONSTRAINT [PK_Categories1]
    PRIMARY KEY CLUSTERED ([CategoryID] ASC);
GO

-- Creating primary key on [QVH_ID] in table 'QuestionVotingHistories'
ALTER TABLE [dbo].[QuestionVotingHistories]
ADD CONSTRAINT [PK_QuestionVotingHistories]
    PRIMARY KEY CLUSTERED ([QVH_ID] ASC);
GO

-- Creating primary key on [AVH_ID] in table 'AnswerVotingHistories'
ALTER TABLE [dbo].[AnswerVotingHistories]
ADD CONSTRAINT [PK_AnswerVotingHistories]
    PRIMARY KEY CLUSTERED ([AVH_ID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [QCommentID] in table 'QuestionComments'
ALTER TABLE [dbo].[QuestionComments]
ADD CONSTRAINT [PK_QuestionComments]
    PRIMARY KEY CLUSTERED ([QCommentID] ASC);
GO

-- Creating primary key on [ACommentID] in table 'AnswerComments'
ALTER TABLE [dbo].[AnswerComments]
ADD CONSTRAINT [PK_AnswerComments]
    PRIMARY KEY CLUSTERED ([ACommentID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [QuestionID] in table 'Answers1'
ALTER TABLE [dbo].[Answers1]
ADD CONSTRAINT [FK_QuestionsAnswers]
    FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[Questions1]
        ([QuestionID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionsAnswers'
CREATE INDEX [IX_FK_QuestionsAnswers]
ON [dbo].[Answers1]
    ([QuestionID]);
GO

-- Creating foreign key on [QuestionID] in table 'QuestionComments'
ALTER TABLE [dbo].[QuestionComments]
ADD CONSTRAINT [FK_QuestionsQuestionComment]
    FOREIGN KEY ([QuestionID])
    REFERENCES [dbo].[Questions1]
        ([QuestionID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionsQuestionComment'
CREATE INDEX [IX_FK_QuestionsQuestionComment]
ON [dbo].[QuestionComments]
    ([QuestionID]);
GO

-- Creating foreign key on [AnswerID] in table 'AnswerComments'
ALTER TABLE [dbo].[AnswerComments]
ADD CONSTRAINT [FK_AnswersAnswerComment]
    FOREIGN KEY ([AnswerID])
    REFERENCES [dbo].[Answers1]
        ([AnswerID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AnswersAnswerComment'
CREATE INDEX [IX_FK_AnswersAnswerComment]
ON [dbo].[AnswerComments]
    ([AnswerID]);
GO

-- Creating foreign key on [UserID] in table 'Questions1'
ALTER TABLE [dbo].[Questions1]
ADD CONSTRAINT [FK_UserQuestions]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserQuestions'
CREATE INDEX [IX_FK_UserQuestions]
ON [dbo].[Questions1]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'Answers1'
ALTER TABLE [dbo].[Answers1]
ADD CONSTRAINT [FK_UserAnswers]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAnswers'
CREATE INDEX [IX_FK_UserAnswers]
ON [dbo].[Answers1]
    ([UserID]);
GO

-- Creating foreign key on [Question_QuestionID] in table 'QuestionVotingHistories'
ALTER TABLE [dbo].[QuestionVotingHistories]
ADD CONSTRAINT [FK_QuestionsQuestionVotingHistory]
    FOREIGN KEY ([Question_QuestionID])
    REFERENCES [dbo].[Questions1]
        ([QuestionID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionsQuestionVotingHistory'
CREATE INDEX [IX_FK_QuestionsQuestionVotingHistory]
ON [dbo].[QuestionVotingHistories]
    ([Question_QuestionID]);
GO

-- Creating foreign key on [Answer_AnswerID] in table 'AnswerVotingHistories'
ALTER TABLE [dbo].[AnswerVotingHistories]
ADD CONSTRAINT [FK_AnswersAnswerVotingHistory]
    FOREIGN KEY ([Answer_AnswerID])
    REFERENCES [dbo].[Answers1]
        ([AnswerID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AnswersAnswerVotingHistory'
CREATE INDEX [IX_FK_AnswersAnswerVotingHistory]
ON [dbo].[AnswerVotingHistories]
    ([Answer_AnswerID]);
GO

-- Creating foreign key on [UserID] in table 'QuestionComments'
ALTER TABLE [dbo].[QuestionComments]
ADD CONSTRAINT [FK_UserQuestionComment]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserQuestionComment'
CREATE INDEX [IX_FK_UserQuestionComment]
ON [dbo].[QuestionComments]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'AnswerComments'
ALTER TABLE [dbo].[AnswerComments]
ADD CONSTRAINT [FK_UserAnswerComment]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAnswerComment'
CREATE INDEX [IX_FK_UserAnswerComment]
ON [dbo].[AnswerComments]
    ([UserID]);
GO

-- Creating foreign key on [Category_CategoryID] in table 'Questions1'
ALTER TABLE [dbo].[Questions1]
ADD CONSTRAINT [FK_QuestionsCategories]
    FOREIGN KEY ([Category_CategoryID])
    REFERENCES [dbo].[Categories1]
        ([CategoryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionsCategories'
CREATE INDEX [IX_FK_QuestionsCategories]
ON [dbo].[Questions1]
    ([Category_CategoryID]);
GO

-- Creating foreign key on [AnswerVotingHistory_AVH_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserAnswerVotingHistory]
    FOREIGN KEY ([AnswerVotingHistory_AVH_ID])
    REFERENCES [dbo].[AnswerVotingHistories]
        ([AVH_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAnswerVotingHistory'
CREATE INDEX [IX_FK_UserAnswerVotingHistory]
ON [dbo].[Users]
    ([AnswerVotingHistory_AVH_ID]);
GO

-- Creating foreign key on [QuestionVotingHistory_QVH_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserQuestionVotingHistory]
    FOREIGN KEY ([QuestionVotingHistory_QVH_ID])
    REFERENCES [dbo].[QuestionVotingHistories]
        ([QVH_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserQuestionVotingHistory'
CREATE INDEX [IX_FK_UserQuestionVotingHistory]
ON [dbo].[Users]
    ([QuestionVotingHistory_QVH_ID]);
GO

-- Default DateTimeNow() as Default value for created row
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT CONSTRAINT_NAME 
DEFAULT GETDATE() FOR [QuestionDTS]

ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT CONSTRAINT_NAME 
DEFAULT GETDATE() FOR [AnswersDTS]

ALTER TABLE [dbo].[QuestionComment]
ADD CONSTRAINT CONSTRAINT_NAME 
DEFAULT GETDATE() FOR QCommentDTS

ALTER TABLE [dbo].[AnswerComment]
ADD CONSTRAINT CONSTRAINT_NAME 
DEFAULT GETDATE() FOR ACommentDTS

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------