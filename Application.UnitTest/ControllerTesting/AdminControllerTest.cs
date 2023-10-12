using Application.API.Controllers;
using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Interfaces;
using Application.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class AdminControllerTest
    {
        [Fact]
        public async Task UpdateUser_ReturnSuccess()
        {
            var userId = Guid.NewGuid();
            var output = "Action Successful";
            var userUpdateDto = new UserUpdateDTO
            {
                Username = "Dummy",
                Email = "dummy@gmail.com",
                Password = "dummy@*#5",
                ProfilePicture = null,
                RoleId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid()
            };

            var existingUserId = Guid.Parse("093EC28D-3900-4556-BAAE-36D4D251DAEA"); 
            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(existingUserId, userUpdateDto))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);

            var result = await adminService.UpdateUser(existingUserId, userUpdateDto);
            Assert.Equal(output, result);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound()
        {
            var userId = Guid.NewGuid(); 
            var userUpdateDto = new UserUpdateDTO();
            var output = "Not Found";

            var existingUserId = Guid.Parse("093EC28D-3900-4556-BAAE-36D4D251DAEA"); 
            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(existingUserId, userUpdateDto))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);


            var result = await adminService.UpdateUser(existingUserId, userUpdateDto);
            Assert.Equal(output, result);
        }

        [Fact]
        public async Task UpdateUser_ReturnNull()
        {
            var userId = Guid.NewGuid();
            UserUpdateDTO? userUpdateDTO = null;
            string output = null;

            var existingUserId = Guid.Parse("093EC28D-3900-4556-BAAE-36D4D251DAEA"); 
            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(existingUserId, userUpdateDTO))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);


            var result = await adminService.UpdateUser(existingUserId, userUpdateDTO);
            Assert.Equal(output, result);
        }

        [Fact]
        public async Task GetUserById_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserDTO
            {
                UserId = userId,
                Username = "Dummy",
                Email = "dummy@gmail.com",
                ProfilePicture = new byte[] { 1, 2, 8, 9 },
                RoleName = "User",
                DepartmentName = "Service",
                Status = 2
            };

            var adminServiceMock = new Mock<IAdminService>();
            adminServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync(user);

            var controller = new AdminController(adminServiceMock.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(user, returnedUser);
        }

        [Fact]
        public async Task GetUserById_ValidEmptyUserId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.Empty;

            var adminServiceMock = new Mock<IAdminService>();
            adminServiceMock.Setup(service => service.GetUserById(userId)).ReturnsAsync((UserDTO)null);

            var controller = new AdminController(adminServiceMock.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var output = "Action Successful";

            var mockService = new Mock<IAdminService>();
            mockService.Setup(service => service.DeleteUser(userId)).ReturnsAsync(output);

            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(output, okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var output = "User not found";

            var mockService = new Mock<IAdminService>();
            mockService.Setup(service => service.DeleteUser(userId)).ReturnsAsync(output);

            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.DeleteUser(userId) ;

            // Assert
            var okResult = Assert.IsType<NotFoundResult>(result);
            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, okResult.StatusCode);
        }
    }
}
