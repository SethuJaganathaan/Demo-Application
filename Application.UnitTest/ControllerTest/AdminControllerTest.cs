using Application.API.Controllers;
using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
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
        public async Task GetUserById_ReturnOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockService = new Mock<IAdminService>();
            mockService.Setup(set => set.GetUserById(userId))
                .ReturnsAsync(new UserDTO
                {
                    UserId = userId,
                    RoleName = "User",
                    DepartmentName = "Product",
                    Username = "Zoro",
                    Email = "zoro@gmail.com",
                    ProfilePicture = new byte[] { 1, 5, 7, 6 },
                    Status = 1
                });
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetUserById(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
            var response = result.Value as UserDTO;
            Assert.NotNull(response);
            Assert.Equal(userId, response.UserId);
            Assert.Equal("User", response.RoleName);
            Assert.Equal("Product", response.DepartmentName);
            Assert.Equal("Zoro", response.Username);
            Assert.Equal("zoro@gmail.com", response.Email);
            Assert.Equal(new byte[] { 1, 5, 7, 6 }, response.ProfilePicture);
            Assert.Equal(1, response.Status);
        }

        [Fact]
        public async Task GetUserById_ReturnBadRequest()
        {
            var userId = Guid.NewGuid();
            var mockService = new Mock<IAdminService>();
            mockService.Setup(set => set.GetUserById(userId))
                .ReturnsAsync((UserDTO?)null); 
            var controller = new AdminController(mockService.Object);

            // Act
            var result = await controller.GetUserById(userId) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, result.StatusCode);
        }

    }
}
