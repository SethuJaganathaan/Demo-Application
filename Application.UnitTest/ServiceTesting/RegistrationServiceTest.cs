using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTest.ServiceTest
{
    public class RegistrationServiceTest
    {
        [Fact]
        public async Task SignUp_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var registrationRepositoryMock = new Mock<IRegistrationRepository>();
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "sanji.jpg");
            var user = new AdminUserCreateDTO
            {
                Username = "Akakami no Shanks",
                Email = "shanks@example.com",
                Password = "Shanks@123",
                RoleId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                ProfilePicture = profilePicture
            };

            registrationRepositoryMock.Setup(repo => repo.SignUp(user))
                .ReturnsAsync("Action Successful");

            var jwtSetting = new JWTSetting { SecurityKey = "myJWTtokenfullofCraps" };
            var registrationService = new RegistrationService(registrationRepositoryMock.Object, Options.Create(jwtSetting));

            // Act
            var result = await registrationService.SignUp(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Action Successful", result);
        }

        [Fact]
        public async Task SignUp_EmailAlreadyExists()
        {
            // Arrange
            var registrationRepositoryMock = new Mock<IRegistrationRepository>();
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "sanji.jpg");
            var user = new AdminUserCreateDTO
            {
                Username = "Sanji",
                Email = "sanji@gmail.com",
                Password = "sanji@123",
                RoleId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid(),
                ProfilePicture = profilePicture
            };
            
            registrationRepositoryMock.Setup(repo => repo.SignUp(user))
                .ReturnsAsync("Email already exist");

            var jwtSetting = new JWTSetting { SecurityKey = "myJWTtokenfullofCraps" };
            var registrationService = new RegistrationService(registrationRepositoryMock.Object, Options.Create(jwtSetting));

            // Act
            var result = await registrationService.SignUp(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Email already exist", result);
        }

        [Fact]
        public async Task Login_ValidUser_ReturnsResponseWithToken()
        {
            // Arrange
            var registrationRepositoryMock = new Mock<IRegistrationRepository>();
            var login = new LoginDTO
            {
                email = "sanji@gamil.com",
                password = "Sanji@123"
            };
            registrationRepositoryMock.Setup(repo => repo.Login(login))
                .ReturnsAsync(new BaseResponse { Status = "LOGIN SUCCESSFUL" });

            registrationRepositoryMock.Setup(repo => repo.GetUser(login.email))
                .ReturnsAsync(new UserRequest
                {
                    UserId = Guid.NewGuid(),
                    RoleId = Guid.NewGuid(),
                    Email = login.email,
                    Username = "Sanji",
                    RoleName = "User"
                });

            var jwtSetting = new JWTSetting { SecurityKey = "MyJWTtokenIhaveItWhenISeeIt" };
            var registrationService = new RegistrationService(registrationRepositoryMock.Object, Options.Create(jwtSetting));

            // Act
            var result = await registrationService.Login(login);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("LOGIN SUCCESSFUL", result.Status);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Login_Failed_ReturnsFailureStatus()
        {
            // Arrange
            var registrationRepositoryMock = new Mock<IRegistrationRepository>();
            var loginDto = new LoginDTO
            {
                email = "zoro@gmail.com", 
                password = "Zoro@123"
            };
            registrationRepositoryMock.Setup(repo => repo.Login(loginDto))
                .ReturnsAsync(new BaseResponse { Status = "Failure" });

            var jwtSetting = new JWTSetting { SecurityKey = "YourSecurityKey" };
            var registrationService = new RegistrationService(registrationRepositoryMock.Object, Options.Create(jwtSetting));

            // Act
            var result = await registrationService.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Failure", result.Status);
            Assert.Null(result.Token);
        }
    }
}
