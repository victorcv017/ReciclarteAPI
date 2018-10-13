CREATE TABLE [dbo].[Centers]
(
	[ID] INT IDENTITY,
  [Schedule] VARCHAR(10) NOT NULL,
  [Location] VARCHAR(30) NOT NULL,
  [Name] VARCHAR(30) NOT NULL,
  [Address] int NOT NULL,
  PRIMARY KEY (ID),
  FOREIGN KEY ([Address]) REFERENCES Addresses(ID)
)
