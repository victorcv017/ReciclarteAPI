CREATE TABLE [dbo].[Purchases]
(
	[ID] INT NOT NULL IDENTITY,
	[Quantity] INT NOT NULL,
	[Transaction] INT NOT NULL,
	[Item] INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY ([Transaction]) REFERENCES Transactions(ID),
	FOREIGN KEY (Item) REFERENCES Items(ID)
)
