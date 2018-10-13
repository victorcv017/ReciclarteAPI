CREATE TABLE [dbo].[Offices]
(
	[ID] INT,
	[Name] VARCHAR(30) NOT NULL,
	[Location] VARCHAR(50) NOT NULL,
	[Schedule] VARCHAR(10) NOT NULL,
	[Enterprise] INT NOT NULL,
	PRIMARY KEY (ID),
  FOREIGN KEY (Enterprise) REFERENCES Enterprises(ID)
)
