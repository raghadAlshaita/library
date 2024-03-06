CREATE TABLE dbo.Book (
    Id INT IDENTITY(1, 1)  PRIMARY KEY,
    Title VARCHAR(50) NOT NULL,
    Author VARCHAR(30) NOT NULL,
    ISBN VARCHAR(30) UNIQUE NOT NULL,
    Status TINYINT DEFAULT 0 CHECK (Status IN (0, 1)), 
	CreatedAt DATETIME DEFAULT GETDATE() NOT NULL, 
    DeletedAt DATETIME NULL, 
	IsVertualDeleted TINYINT DEFAULT 0 CHECK (IsVertualDeleted IN (0, 1)),
);

CREATE TABLE dbo.Users (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL,
	IsActive BIT NOT NULL DEFAULT 0,
	CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    DeletedAt DATETIME NULL, 
	IsVertualDeleted TINYINT DEFAULT 0 CHECK (IsVertualDeleted IN (0, 1)),
);

CREATE TABLE UserRoles
(
 Id INT PRIMARY KEY IDENTITY(1, 1),
 UserID INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ,
 RoleID INT NOT NULL FOREIGN KEY REFERENCES Role(Id),
 CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
 DeletedAt DATETIME NULL, 
 IsVertualDeleted TINYINT DEFAULT 0 CHECK (IsVertualDeleted IN (0, 1)),
);

CREATE TABLE Role(
Id INT PRIMARY KEY IDENTITY(1, 1),
RoleName varchar(50),
);



CREATE TABLE dbo.Borrowing (
	Id INT PRIMARY KEY IDENTITY(1, 1) ,
    UserId INT FOREIGN KEY REFERENCES dbo.Users ON DELETE CASCADE,
    BookId INT FOREIGN KEY REFERENCES dbo.Book ON DELETE CASCADE,
	CreatedAt DATETIME DEFAULT GETDATE() NOT NULL, 
    DeletedAt DATETIME NULL, 
	IsVertualDeleted TINYINT DEFAULT 0 CHECK (IsVertualDeleted IN (0, 1)),
);

CREATE TABLE Tokens (
    Id INT PRIMARY KEY IDENTITY(1, 1),
    UserId INT NOT NULL REFERENCES dbo.Users(Id),
    Token VARCHAR(255) NOT NULL UNIQUE, 
    ExpiryDate DATETIME NOT NULL
);




	


--books procedure 
--get All

CREATE  PROCEDURE  GetAll 
@col NVARCHAR(50)=Null, 
@searchTerm NVARCHAR(50) = NULL,
@PageNumber INT = 1,
@RowsPerPage INT = 5
AS
BEGIN
SET NOCOUNT ON;
    IF (@col IS NULL OR @searchTerm IS NULL )BEGIN
	  SELECT * FROM Book b
	where b.IsVertualDeleted=0
	order by b.Id desc
	OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
   FETCH NEXT @RowsPerPage ROWS ONLY;
        RETURN;
    END;
	ELSE 
	BEGIN
	SELECT * FROM Book b
	where b.IsVertualDeleted=0 AND QUOTENAME(@col)  LIKE Concat('%',@searchTerm,'%') 
	order by b.Id desc
	OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
   FETCH NEXT @RowsPerPage ROWS ONLY;
        RETURN;
END;
END;
GO                     
--create
CREATE PROCEDURE AddNewBook @title NVARCHAR(MAX), @author NVARCHAR(MAX), @isbn CHAR(13) AS
BEGIN
DECLARE @bookId INT;
    IF EXISTS (SELECT * FROM Book WHERE ISBN = @isbn) BEGIN
        RAISERROR ('Book already exists with this ISBN.', 16, 1);
        RETURN;
    END

    INSERT INTO Book (Title, Author, ISBN) VALUES (@title, @author, @isbn);
	    select  SCOPE_IDENTITY() as bookId ;
END;
GO

--update 
CREATE PROCEDURE UpdateBookDetails @bookId INT, @title NVARCHAR(MAX), @author NVARCHAR(MAX), @isbn CHAR(13) AS
BEGIN 
    IF NOT EXISTS (SELECT * FROM Book WHERE Id = @bookId) BEGIN
        RAISERROR ('Book with specified ID does not exist.', 16, 1);
        RETURN;
    END
	    IF EXISTS (SELECT * FROM Book WHERE ISBN = @isbn and Id !=@bookId) BEGIN
        RAISERROR ('Book already exists with this ISBN.', 16, 1);
        RETURN;
    END

    UPDATE Book SET Title = COALESCE(@title, Title), Author = COALESCE(@author, Author), ISBN = COALESCE(@isbn, ISBN) WHERE Id = @bookId;
	   SELECT SCOPE_IDENTITY()
END;
GO
--delete
CREATE PROCEDURE DeleteBook @bookId int
AS
BEGIN
  IF NOT EXISTS (SELECT * FROM Book WHERE Id = @bookId) BEGIN
        RAISERROR ('Book with specified ID does not exist.', 16, 1);
        RETURN;
    END

    UPDATE Book SET IsVertualDeleted=1,DeletedAt= GETDATE()  WHERE Id = @bookId;
	   SELECT SCOPE_IDENTITY()
END;
GO
--get by id
CREATE PROCEDURE GetBookByID @param int AS
BEGIN
IF NOT EXISTS (SELECT * FROM Book WHERE Id = @param) BEGIN
        RAISERROR ('Book with specified Id does not exist.', 16, 1);
        RETURN;
    END
    SELECT * FROM Book b WHERE Id = @param;
END;
GO
--get by ispn
CREATE PROCEDURE GetBookByISBN @param int AS
BEGIN
IF NOT EXISTS (SELECT * FROM Book WHERE ISBN = @param) BEGIN
        RAISERROR ('Book with specified ISBN does not exist.', 16, 1);
        RETURN;
    END
    SELECT * FROM Book b WHERE ISBN = @param;
END;
GO
--get by title
CREATE PROCEDURE GetBookByTitle @param NVARCHAR(MAX) As
BEGIN
  IF NOT EXISTS (SELECT * FROM Book WHERE Title = @param) BEGIN
        RAISERROR ('Book with specified Title does not exist.', 16, 1);
        RETURN;
    END
 SELECT * FROM Book b WHERE Title = @param;
END;
GO
--get by status
CREATE PROCEDURE GetBookByStatus @param int AS
BEGIN

    SELECT * FROM Book b WHERE Status = @param;
END;
GO
--get by author
CREATE PROCEDURE GetBookByAuthor @param NVARCHAR(MAX) As
BEGIN
  IF NOT EXISTS (SELECT * FROM Book WHERE Author = @param) BEGIN
        RAISERROR ('Book with specified Author Name does not exist.', 16, 1);
        RETURN;
    END
 SELECT * FROM Book b WHERE Author = @param;
END;
GO


--borrow 
CREATE PROCEDURE BorrowBook
@bookId INT,
@username varchar(50)

AS
BEGIN
DECLARE @current_status BIT;
DECLARE @userId int;
    SET NOCOUNT ON;
	
    
    IF NOT EXISTS (SELECT * FROM Book WHERE Id = @bookId)
    BEGIN
        RAISERROR ('Book with specified ID does not exist.', 16, 1);
    END


	
    SET @current_status = (SELECT ISNULL((CASE WHEN Status = 1 THEN 1 ELSE 0 END), 0) FROM Book WHERE Id = @bookId);

    IF @current_status = 1
    BEGIN
        RAISERROR ('Book already borrowed.', 16, 1);
        RETURN;
    END

	IF NOT EXISTS (SELECT * FROM Users WHERE UserName = @username) BEGIN
        RAISERROR ('User with specified User  Name does not exist.', 16, 1);
        RETURN;
    END
	set @userId= (SELECT Id FROM Users WHERE UserName = @username AND IsVertualDeleted!=1 );

    INSERT INTO Borrowing(BookId, UserId)
    VALUES (@bookId, @userId);
	SELECT SCOPE_IDENTITY() as id;


    UPDATE Book
    SET Status = 1
    WHERE Book.Id = @bookId;
END;
Go



CREATE PROCEDURE GetBooksBorrowingByUser
@UserID INT 
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM Users WHERE Id = @UserID)
    BEGIN
        RAISERROR ('User with specified ID does not exist.', 16, 1);
    END

	SELECT u.Id , bk.Id AS BookID
	FROM Users u
	JOIN Borrowing b ON b.UserId = u.Id
	JOIN Book bk ON bk.Id = b.BookId
	WHERE u.Id =@UserID AND bk.Status !=0;
END;

GO

CREATE PROCEDURE ReturnBook 
@bookId INT
AS
BEGIN 
SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM Book WHERE Id = @bookId And Status =1)
    BEGIN
        RAISERROR ('Book with specified ID does not exist.', 16, 1);
    END
	UPDATE Book  SET Status = 0 WHERE  Book.id=@bookId;
END;
Go

CREATE PROCEDURE IsBookBorrowedByThisUser 
@bookId int,
@username varchar(50)
As 
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT * FROM Users WHERE UserName = @username)
    BEGIN
        RAISERROR ('User with specified User name does not exist.', 16, 1);
    END

	SELECT u.Id , bk.Id AS BookID
	FROM Users u
	JOIN Borrowing b ON b.UserId = u.Id
	JOIN Book bk ON bk.Id = b.BookId
	WHERE u.UserName =@username AND bk.Status !=0 AND bk.Id=@bookId ;
END;
	


----User 

CREATE PROCEDURE GetUserByID @param int AS
BEGIN
IF NOT EXISTS (SELECT * FROM Users WHERE Id = @param) BEGIN
        RAISERROR ('User with specified Id does not exist.', 16, 1);
        RETURN;
    END
    SELECT * FROM Users WHERE Id = @param AND IsVertualDeleted!=1 ;
END;
GO

--create
CREATE PROCEDURE AddNewUser @username NVARCHAR(MAX), @password NVARCHAR(MAX) AS
BEGIN

    IF EXISTS (SELECT * FROM Users WHERE UserName = @username AND IsVertualDeleted=0) BEGIN
        RAISERROR ('User Name already exists.', 16, 1);
        RETURN;
    END

  INSERT INTO Users (UserName, Password) VALUES (@username, HASHBYTES('SHA2_256', @password));
		   select  SCOPE_IDENTITY() as userId ;
END;
GO

--get all

Create  PROCEDURE  GetAllUser
@col NVARCHAR(50)=Null, 
@searchTerm NVARCHAR(50) = NULL,
@PageNumber INT = 1,
@RowsPerPage INT = 5
AS
BEGIN
SET NOCOUNT ON;
    IF (@col IS NULL OR @searchTerm IS NULL )BEGIN
	  SELECT * FROM Users u
	where u.IsVertualDeleted=0
	order by u.Id desc
	OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
   FETCH NEXT @RowsPerPage ROWS ONLY;
        RETURN;
    END;
	ELSE 
	BEGIN
	SELECT * FROM Users u
	where u.IsVertualDeleted=0 AND @col LIKE Concat('%',@searchTerm,'%') 
	order by u.Id desc
	OFFSET (@PageNumber - 1) * @RowsPerPage ROWS
   FETCH NEXT @RowsPerPage ROWS ONLY;
        RETURN;
END;
END;
GO   
------
CREATE PROCEDURE SetRolesForUser @userId int,@adminRole bit=NULL AS
BEGIN
    IF EXISTS (SELECT * FROM Users WHERE Id = @userId) BEGIN
        IF (@adminRole IS NULL OR @adminRole=0) BEGIN
            INSERT INTO UserRoles (UserID,RoleID) VALUES (@userId,2);
			End;
        ELSE BEGIN
            INSERT INTO UserRoles (UserID,RoleID) VALUES (@userId,1), (@userId,2);
			END;
		End;
    ELSE BEGIN
        RAISERROR ('User Not Found.', 16, 1);
        RETURN;
    END;
END;
GO

----CheckCredentials 
CREATE PROCEDURE CheckCredentials @username NVARCHAR(MAX), @password NVARCHAR(MAX) AS
BEGIN

 
	 DECLARE @userId INT;

   IF EXISTS(SELECT 1 FROM Users WHERE UserName = @username AND Password = HASHBYTES('SHA2_256', @Password))
        SET @userId = (SELECT Id FROM Users WHERE UserName = @Username AND Password =  HASHBYTES('SHA2_256', @Password));
    ELSE
        SET @userId = 0; 

    SELECT @userId AS Result;
         END ;

GO
-----

CREATE PROCEDURE DeleteUser @userId int1ED 
AS
BEGIN
  IF NOT EXISTS (SELECT * FROM Users WHERE Id = @userId) BEGIN
        RAISERROR ('User with specified ID does not exist.', 16, 1);
        RETURN;
    END

	IF EXISTS (SELECT 1 FROM Users u WHERE u.Id = @userId AND u.IsVertualDeleted = 1) BEGIN
       RAISERROR ('User is marked deleted.', 16, 1);
      RETURN;
    END

    UPDATE Users SET IsVertualDeleted=1,DeletedAt= GETDATE()  WHERE Id = @userId;
	   SELECT SCOPE_IDENTITY()  as id
END;
GO


CREATE PROCEDURE GetUserByUserName @param varchar(50) AS
BEGIN
IF NOT EXISTS (SELECT * FROM Users WHERE UserName = @param) BEGIN
        RAISERROR ('User with specified User  Name does not exist.', 16, 1);
        RETURN;
    END
    SELECT * FROM Users WHERE UserName = @param AND IsVertualDeleted!=1 ;
END;
GO

CREATE PROCEDURE SaveJWTToken
   @token VARCHAR(MAX),
   @userId INT
AS
BEGIN
   DECLARE @expiryDateTime DATETIME = DATEADD(day, 30, GETDATE());
   DECLARE @trimmedToken NVARCHAR(MAX) = RIGHT(@token, CHARINDEX('.', REVERse(@token)) - 1);

   INSERT INTO Tokens (Token, UserId, ExpiryDate) VALUES(@trimmedToken, @userId, @expiryDateTime);
END;
