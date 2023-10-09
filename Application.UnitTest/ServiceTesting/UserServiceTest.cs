using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ServiceTest
{
    public class UserServiceTest
    {
        [Fact]
        public async Task GetAllUsers_ReturnsListOfUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAllUsers())
                .ReturnsAsync(new List<UserDTO>
                {
                    new UserDTO
                    {
                        UserId = Guid.NewGuid(),
                        RoleName = "User",
                        DepartmentName = "Product",
                        Username = "Zoro",
                        Email = "zoro@gmail.com",
                        ProfilePicture = new byte[] { 1, 5, 7, 6 },
                        Status = 1
                    },
                    new UserDTO
                    {
                        UserId = Guid.NewGuid(),
                        RoleName = "Admin",
                        DepartmentName = "Service",
                        Username = "Luffy",
                        Email = "luffy@gmail.com",
                        ProfilePicture = new byte[] { 2, 6, 8, 9 },
                        Status = 1
                    }
                });

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserDTO>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task SoftDeleteUser_ValidUserId_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.SoftDeleteUser(userId))
                .ReturnsAsync(true);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.SoftDeleteUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result);
        }

        [Fact]
        public async Task SoftDeleteUser_InvalidUserId_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.SoftDeleteUser(userId))
                .ReturnsAsync(false);

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = await userService.SoftDeleteUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result);
        }
    }
}
