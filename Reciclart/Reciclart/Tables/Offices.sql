CREATE TABLE [dbo].[Offices]
(
	[ID] INT,
	[Name] VARCHAR(30) NOT NULL,
	[Address] int NOT NULL,
	[Enterprise] INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (Enterprise) REFERENCES Enterprises(ID),
	FOREIGN KEY ([Address]) REFERENCES Addresses(ID)
)
