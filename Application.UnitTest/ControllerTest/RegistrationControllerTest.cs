using Application.API.Controllers;
using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class RegistrationControllerTest
    {
        [Fact]
        public async Task SignUp_ValidUser_ReturnsOk()
        {
            // Arrange
            var mockRegistrationService = new Mock<IRegistrationService>();
            mockRegistrationService.Setup(service => service.SignUp(It.IsAny<AdminUserCreateDTO>()))
                .ReturnsAsync("Action Successful");
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "profile.jpg");
            var controller = new RegistrationController(mockRegistrationService.Object);

            // Act
            var user = new AdminUserCreateDTO
            {
                Username = "Sanji",
                Email = "sanji@gmail.com",
                Password = "Dummy@123",
                RoleId = Guid.Parse("7F89D12F-2383-42FF-9B26-63132C92A7A8"),
                DepartmentId = Guid.Parse("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"),
                ProfilePicture = profilePicture
            };
            var result = await controller.SignUp(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task SignUp_AlreadyExistEmail()
        {
            // Arrange
            var mockService = new Mock<IRegistrationService>();
            string output = "Email already exist";
            mockService.Setup(set => set.SignUp(It.IsAny<AdminUserCreateDTO>()))
                .ReturnsAsync(output);

            var controller = new RegistrationController(mockService.Object);
            var user = new AdminUserCreateDTO
            {
                Email = "sanji@gmail.com",
            };

            // Act
            var result = await controller.SignUp(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials()
        {
            // Arrange
            var mockRegistrationService = new Mock<IRegistrationService>();
            mockRegistrationService.Setup(service => service.Login(It.IsAny<LoginDTO>()))
                .ReturnsAsync(new BaseResponse
                {
                    Status = "LOGIN SUCCESSFUL",
                    Token = "Dummy-token-generate"
                });
            var controller = new RegistrationController(mockRegistrationService.Object);

            // Act
            var loginDto = new LoginDTO
            {
                email = "dummyuser@gmail.com",
                password = "User@123"
            };
            var result = await controller.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidCredentials()
        {
            // Arrange
            var mockRegistrationService = new Mock<IRegistrationService>();
            mockRegistrationService.Setup(service => service.Login(It.IsAny<LoginDTO>()))
                .ReturnsAsync(new BaseResponse
                {
                    Status = "Invalid email or password",
                });

            var controller = new RegistrationController(mockRegistrationService.Object);

            // Act
            var loginDto = new LoginDTO
            {
                email = "dumy@outlook.com",
                password = "111111"
            };
            var result = await controller.Login(loginDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetUser_ValidEmail()
        {
            // Arrange
            var mockRegistrationService = new Mock<IRegistrationService>();
            mockRegistrationService.Setup(service => service.GetUser(It.IsAny<string>()))
                .ReturnsAsync(new UserRequest
                {
                    UserId = Guid.NewGuid(),
                    Email = "test@gmail.com",
                });

            var controller = new RegistrationController(mockRegistrationService.Object);

            // Act
            var email = "test@gmail.com"; 
            var result = await controller.GetUser(email) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetUser_InvalidEmail_ReturnsNotFound()
        {
            // Arrange
            var mockRegistrationService = new Mock<IRegistrationService>();
            mockRegistrationService.Setup(service => service.GetUser(It.IsAny<string>()))
                .ReturnsAsync((UserRequest)null);

            var controller = new RegistrationController(mockRegistrationService.Object);

            // Act
            var email = "dummy@gmail.com"; 
            var result = await controller.GetUser(email) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}

