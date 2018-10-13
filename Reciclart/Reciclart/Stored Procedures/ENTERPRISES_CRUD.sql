--select

CREATE PROCEDURE dbo.SELECT_ENTERPRISE
	@id int
	as
	begin
	Select * from [dbo].[Enterprises]
	where [Enterprises].[ID]=@id
	end
	go

CREATE PROCEDURE dbo.SELECT_ALL_ENTERPRISES
	as
	begin
	Select * from [dbo].[Enterprises]
	end
	go

--insert
CREATE PROCEDURE dbo.INSERT_ENTERPRISE
	@id int,@name varchar(20),  @password varchar(20), @email varchar(40),@balance int
	as
	begin
	INSERT into [dbo].[Enterprises]
	values(@id,@name,@password,@email,@balance)
	end
	go

--update
CREATE PROCEDURE dbo.UPDATE_ENTERPRISE
	@id int, @password varchar(20), @email varchar(40),@balance int
	as
	begin
	UPDATE [dbo].[Enterprises]
	set [Password]=@password,
	[Email]=@email,
	[Balance]=@balance
	where [ID]=@id
	end
	go

--delete
CREATE PROCEDURE dbo.DELETE_ENTERPRISE
	@id int
	as
	begin
	DELETE FROM [dbo].[Enterprises] where [ID]=@id
	end

