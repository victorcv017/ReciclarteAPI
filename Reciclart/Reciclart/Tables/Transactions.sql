﻿CREATE TABLE [dbo].[Transactions]
(
	[ID] INT IDENTITY,
	[Amount] INT NOT NULL,
	[Date] DATE NOT NULL,
	[User] INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY ([User]) REFERENCES Users(ID)
)
