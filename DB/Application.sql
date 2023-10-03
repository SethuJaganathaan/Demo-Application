USE Application

INSERT INTO Role VALUES(NEWID(),'User'),(NEWID(),'SuperAdmin')

INSERT INTO Department VALUES(NEWID(),'Data'),(NEWID(),'Product'),(NEWID(),'Service')

DECLARE @UseriId UNIQUEIDENTIFIER = NEWID();
DECLARE @Username NVARCHAR(50) = 'Luffy';
DECLARE @Email NVARCHAR(100) = 'luffy@gmail.com'
DECLARE @Password NVARCHAR(50) = 'Luffy@123'
DECLARE @Status SMALLINT = 0
DECLARE @Roleid UNIQUEIDENTIFIER = '9FCBF75F-754E-4D47-A9F8-67C535B4FF25'
DECLARE @DepartmentId UNIQUEIDENTIFIER = '113D2E85-E0E6-44B6-85C9-D6059D6B97B6'
DECLARE @ProfilePicture VARBINARY(MAX);

SET @ProfilePicture = (SELECT BulkColumn FROM OPENROWSET(BULK N'D:\ZImages\kidluffy.jpg', SINGLE_BLOB) AS x);

IF SUBSTRING(@ProfilePicture, 1, 2) = 0x8950
    OR SUBSTRING(@ProfilePicture, 1, 2) = 0xFFD8
BEGIN
	INSERT INTO [Users] (UserId, Username, Email, Password, Status, RoleId, DepartmentId, ProfilePicture) 
	VALUES (@UseriId, @Username, @Email, @Password, @Status, @Roleid, @DepartmentId, @ProfilePicture)
END
ELSE
BEGIN
    PRINT 'Only PNG and JPG formats are allowed.';
END;

