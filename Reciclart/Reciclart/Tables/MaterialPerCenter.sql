CREATE TABLE [dbo].[MaterialPerCenter]
(
	[Center] INT NOT NULL,
	[Material] INT NOT NULL,
	FOREIGN KEY (Center) REFERENCES Centers(ID),
	FOREIGN KEY (Material) REFERENCES Materials(ID)
)
