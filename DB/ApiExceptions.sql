CREATE TABLE [dbo].[ApiExceptions]
(
	[Id]			INT				IDENTITY(1,1)		NOT NULL		CONSTRAINT PK_ApiExceptions PRIMARY KEY CLUSTERED,
	[Type]			NVARCHAR(255)	NOT NULL,
	[Message]		Text			NOT NULL,
	[StackTrace]	Text			NOT NULL,
	[Namespace]		NVARCHAR(255)	NOT NULL,
	[Classname]		NVARCHAR(255)	NOT NULL,
	[Method]		NVARCHAR(255)	NOT NULL,
	[CreatedOn]		DateTime2		NOT NULL
)
