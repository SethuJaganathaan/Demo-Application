USE Application

-- User SignUp -- 
CREATE PROCEDURE UserSignup
    @username VARCHAR(255),
    @email VARCHAR(255),
    @password VARCHAR(255),
    @profilepicture VARBINARY(MAX),
    @status SMALLINT,
    @roleid UNIQUEIDENTIFIER,
    @departmentid UNIQUEIDENTIFIER,
    @profilepic_extension VARCHAR(5)
AS
BEGIN
    IF @profilepic_extension IN ('.png', '.jpg', '.jpeg')
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Users WHERE email = @email)
        BEGIN
            INSERT INTO Users (Userid, Username, Email, Password, Profilepicture, Status, RoleId, DepartmentId)
            VALUES (NEWID(), @username, @email, @password, @profilepicture, @status, @roleid, @departmentid)

            SELECT 'Signup successful.' AS Result
        END
        ELSE
        BEGIN
            SELECT 'Email already exists.' AS Result
        END
    END
    ELSE
    BEGIN
        SELECT 'Invalid picture extension.' AS Result
    END
END;

DROP PROC UserSignup

DECLARE @name VARCHAR(50) = 'Dummy';
DECLARE @mail VARCHAR(50) = 'dummy@gmail.com';
DECLARE @pass VARCHAR(50) = 'dummy123';
DECLARE @stat SMALLINT = 0;
DECLARE @Rid UNIQUEIDENTIFIER = '7F89D12F-2383-42FF-9B26-63132C92A7A8';
DECLARE @Did UNIQUEIDENTIFIER = '113D2E85-E0E6-44B6-85C9-D6059D6B97B6';
DECLARE @extension VARCHAR(5) = '.jpg';

DECLARE @profilepicture VARBINARY(MAX); 
SELECT @profilepicture = BulkColumn
FROM OPENROWSET(BULK 'D:\ZImages\kidusopp.jpg', SINGLE_BLOB) AS img;

EXEC UserSignup @name, @mail, @pass, @profilepicture, @stat, @Rid, @Did, @extension;


-- Login --
CREATE PROC UserLogin
	@email VARCHAR(20),
	@Password VARCHAR(20)
AS
BEGIN
	IF EXISTS (SELECT * FROM Users WHERE email = @email AND Password = @Password)
	BEGIN
		SELECT 'LOGIN SUCCESSFUL' AS Result
	END
	ELSE
	BEGIN
		SELECT 'INVALID EMAIL OR PASSWORD' AS Result
	END
END

DROP PROC UserLogin

DECLARE @email VARCHAR(255) = 'blackfoot@gmail.com';
DECLARE @password VARCHAR(255) = 'Sanji@123';
EXEC UserLogin  @email, @password;

-------------------------------------------------------------------------------------------------------------------------------------




