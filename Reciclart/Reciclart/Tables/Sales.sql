CREATE TABLE [dbo].[Sales]
(
	[ID] INT,
	[Weight] INT NOT NULL,
	[Transaction] INT NOT NULL,
	[Center] INT NOT NULL,
	[Material] INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY ([Transaction]) REFERENCES Transactions(ID),
	FOREIGN KEY ([Center]) REFERENCES Centers(ID),
	FOREIGN KEY ([Material]) REFERENCES Materials(ID)
)
