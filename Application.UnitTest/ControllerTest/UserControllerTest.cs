using Application.API.Controllers;
using Application.Repository.DTO.User;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class UserControllerTest
    {
        [Fact]
        public async Task GetAllUsers()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<UserDTO>());

            var controller = new UserController(mockUserService.Object);


            var result = await controller.GetAllUsers() as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<List<UserDTO>>(result.Value);
        }

        [Fact]
        public async Task GetAllUsers_ReturnNull()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync((List<UserDTO>)null); 

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.GetAllUsers() as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task SoftDeleteUser()
        {
            var userId = Guid.NewGuid();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.SoftDeleteUser(userId))
                .ReturnsAsync(true);

            var controller = new UserController(mockUserService.Object);


            var result = await controller.SoftDeleteUser(userId) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public async Task SoftDeleteUser_ValidUserId_ReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockservice = new Mock<IUserService>();
            mockservice.Setup(set => set.SoftDeleteUser(userId)).ReturnsAsync(true);
            var controller = new UserController(mockservice.Object);

            // Act
            var result = await controller.SoftDeleteUser(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public async Task SoftDeleteUser_ValidUserId_ReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockservice = new Mock<IUserService>();
            mockservice.Setup(set => set.SoftDeleteUser(userId)).ReturnsAsync(false);
            var controller = new UserController(mockservice.Object);

            // Act
            var result = await controller.SoftDeleteUser(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.False((bool)result.Value);
        }

        [Fact]
        public async Task SoftDeleteUser_InvalidUserId_ReturnNotFound()
        {
            var invalidUserId = Guid.NewGuid();
            var mockService = new Mock<IUserService>();
            mockService.Setup(set => set.SoftDeleteUser(invalidUserId)).ReturnsAsync((bool?)null);
            var controller = new UserController(mockService.Object);

            var result = await controller.SoftDeleteUser(invalidUserId) as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
