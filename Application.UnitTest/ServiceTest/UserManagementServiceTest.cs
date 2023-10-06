using Application.Repository.DTO.Common;
using Application.Repository.Interfaces;
using Application.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ServiceTest
{
    public class UserManagementServiceTest
    {
        [Fact]
        public async Task UserManagementByRoleId_NormalUser()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var userManagementRepositoryMock = new Mock<IUserManagementRepository>();
            userManagementRepositoryMock.Setup(repo => repo.UserManagementByRoleid(roleId))
                .ReturnsAsync(new
                {
                    UserCount = 5,
                    AdminCount = 2
                });

            var userManagementService = new UserManagementService(userManagementRepositoryMock.Object);

            // Act
            var result = await userManagementService.UserManagementByRoleid(roleId);

            // Assert
            Assert.NotNull(result);
            var userCount = (int)result.GetType().GetProperty("UserCount").GetValue(result);
            var adminCount = (int)result.GetType().GetProperty("AdminCount").GetValue(result);
            Assert.Equal(5, userCount);
            Assert.Equal(2, adminCount);
        }

        [Fact]
        public async Task UserManagementByRoleId_InvalidRoleId()
        {
            // Arrange
            var roleId = Guid.Empty;
            var userManagementRepositoryMock = new Mock<IUserManagementRepository>();
            userManagementRepositoryMock.Setup(repo => repo.UserManagementByRoleid(roleId))
                .ReturnsAsync("Invalid Roleid");

            var userManagementService = new UserManagementService(userManagementRepositoryMock.Object);

            // Act
            var result = await userManagementService.UserManagementByRoleid(roleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid Roleid", result);
        }

        [Fact]
        public async Task UserManagementByRoleId_SuperAdminRole()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var superAdminRoleId = Guid.NewGuid();
            var userManagementRepositoryMock = new Mock<IUserManagementRepository>();
            userManagementRepositoryMock.Setup(repo => repo.UserManagementByRoleid(roleId))
                .ReturnsAsync(new { Message = "SuperAdmin role not found" });

            userManagementRepositoryMock.Setup(repo => repo.UserManagementByRoleid(superAdminRoleId))
                .ReturnsAsync(new[]
                {
                    new AdminUserDTO
                    {
                        UserId = Guid.NewGuid(),
                        DepartmentId = Guid.NewGuid(),
                        Username = "Admin1",
                        Email = "admin1@example.com",
                        ProfilePicture = new byte[] { 1, 2, 3 }
                    },
                    new AdminUserDTO
                    {
                        UserId = Guid.NewGuid(),
                        DepartmentId = Guid.NewGuid(),
                        Username = "Admin2",
                        Email = "admin2@example.com",
                        ProfilePicture = new byte[] { 4, 5, 6 }
                    },
                });

            var userManagementService = new UserManagementService(userManagementRepositoryMock.Object);

            // Act
            var result = await userManagementService.UserManagementByRoleid(superAdminRoleId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AdminUserDTO[]>(result);
            Assert.Equal(2, ((AdminUserDTO[])result).Length);
        }

    }
}
