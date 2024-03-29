﻿CREATE TABLE [dbo].[Items]
(
	[Id]				INT IDENTITY(1,1)	NOT NULL	CONSTRAINT PK_Items			PRIMARY KEY CLUSTERED,
	[Name]				NVARCHAR(50)		NOT NULL	CONSTRAINT UQ_Items_Name	UNIQUE,
	[Stocks]			INT					NOT NULL	DEFAULT 0,
	[UnitPrice]			DECIMAL(7,2)		NOT NULL
)
