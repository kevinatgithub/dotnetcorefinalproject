CREATE TABLE [dbo].[ApiExceptions]
(
	[Id]			INT				IDENTITY(1,1)		NOT NULL		CONSTRAINT PK_ApiExceptions PRIMARY KEY CLUSTERED,
	[Type]			NVARCHAR(50)	NOT NULL,
	[Message]		NVARCHAR(255)	NOT NULL,
	[StackTrace]	Text			NOT NULL,
	[Namespace]		NVARCHAR(50)	NOT NULL,
	[Classname]		NVARCHAR(50)	NOT NULL,
	[Method]		NVARCHAR(50)	NOT NULL,
	[CreatedOn]		DateTime2		NOT NULL
)
