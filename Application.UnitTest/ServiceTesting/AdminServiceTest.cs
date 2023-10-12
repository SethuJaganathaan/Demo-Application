using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Application.UnitTest.ServiceTest
{
    public class AdminServiceTest
    {
        [Fact]
        public async Task DeleteUser_ValidUserId_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.DeleteUser(userId))
                .ReturnsAsync("Action Successful");

            var adminService = new AdminService(adminRepositoryMock.Object);

            // Act
            var result = await adminService.DeleteUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Action Successful", result);
        }

        [Fact]
        public async Task GetUserById_ValidUserId_ReturnsUserDTO()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userDTO = new UserDTO
            {
                UserId = userId,
                RoleName = "User",
                DepartmentName = "Product",
                Username = "Zoro",
                Email = "zoro@gmail.com",
                ProfilePicture = new byte[] { 1, 5, 7, 6 },
                Status = 1
            };
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetUserById(userId))
                .ReturnsAsync(userDTO);

            var adminService = new AdminService(adminRepositoryMock.Object);

            // Act
            var result = await adminService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserDTO>(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task UpdateUser_ValidUserId_ReturnsActionSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "profile.jpg");
            var userUpdateDTO = new UserUpdateDTO
            {
                Username = "Dummy",
                Email = "dump@gmail.com",
                ProfilePicture = profilePicture,
                Password = "Dump@123",
                RoleId = Guid.Parse("113D2E85-E0E6-44B6-85C9-D6059D6B97B6")
            };

            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.UpdateUser(userId, userUpdateDTO))
                .ReturnsAsync("Action Successful");

            var adminService = new AdminService(adminRepositoryMock.Object);

            // Act
            var result = await adminService.UpdateUser(userId, userUpdateDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Action Successful", result);
        }
    }
}
