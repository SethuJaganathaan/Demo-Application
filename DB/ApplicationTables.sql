USE Application

CREATE TABLE Role
(
RoleId UNIQUEIDENTIFIER PRIMARY KEY,
Rolename NVARCHAR(255) NOT NULL
)

CREATE TABLE Department
(
DepartmentId UNIQUEIDENTIFIER PRIMARY KEY,
DepartmentName NVARCHAR(255) NOT NULL
)

CREATE TABLE Users
(
UserId UNIQUEIDENTIFIER PRIMARY KEY,
Username NVARCHAR(255) NOT NULL,
Email NVARCHAR(255) NOT NULL,
Password NVARCHAR(255) NOT NULL,
ProfilePicture VARBINARY(MAX) NOT NULL,
Status SMALLINT NOT NULL,
RoleId UNIQUEIDENTIFIER FOREIGN KEY(RoleId) REFERENCES Role(RoleId),
DepartmentId UNIQUEIDENTIFIER FOREIGN KEY(DepartmentId) REFERENCES Department(DepartmentId),
)

SELECT * FROM Department
SELECT * FROM Role
SELECT * FROM Users
