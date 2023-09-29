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
    }
}
