CREATE TABLE [dbo].[Items]
(
	[ID] INT NOT NULL IDENTITY,
  [Name] VARCHAR(30) NOT NULL,
  [Value] INT NOT NULL,
  [Enterprise] INT NOT NULL,
  PRIMARY KEY (ID),
  FOREIGN KEY (Enterprise) REFERENCES Enterprises(ID)
)
