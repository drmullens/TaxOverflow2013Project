--Delete Order
DELETE FROM AnswerCommentTBLs
DELETE FROM AnswerVotingHistoryTBLs
DELETE FROM QuestionCommentTBLs
DELETE FROM QuestionVotingHistoryTBLs
DELETE FROM AnswerTBLs
DELETE FROM QuestionTBLs
DELETE FROM CategoryTBLs
DELETE FROM UserTBLs


--Restart PKs at 1
DBCC CHECKIDENT ('dbo.AnswerCommentTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.AnswerTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.AnswerVotingHistoryTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.CategoryTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.QuestionCOmmentTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.QuestionTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.QuestionVotingHistoryTBLs', RESEED, 1)
DBCC CHECKIDENT ('dbo.UserTBLs', RESEED, 1)