CREATE TABLE [dbo].[Enterprises]
(
	[ID] INT PRIMARY KEY,
	[Name] Varchar(20) NOT NULL,
	[Password] Varchar(20) NOT NULL,
	[Email] Varchar(40) NOT NULL,
	[Balance] INT NOT NULL,
)
