using Application.API.Controllers;
using Application.Repository.DTO.Common;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class UserManagementControllerTest
    {
        private class UserCounts
        {
            public int UserCount { get; set; }
            public int AdminCount { get; set; }
        }

        [Theory]
        [InlineData("7F89D12F-2383-42FF-9B26-63132C92A7A8")]
        [InlineData("9FCBF75F-754E-4D47-A9F8-67C535B4FF25")]
        public async Task UserManagementByRoleid_ReturnsResult(string roleId)
        {
            // Arrange
            Guid Roleid = Guid.Parse(roleId);
            var mockService = new Mock<IUserManagementService>();

            if (roleId == "7F89D12F-2383-42FF-9B26-63132C92A7A8")
            {
                mockService.Setup(service => service.UserManagementByRoleid(Roleid))
                    .ReturnsAsync(new UserCounts { UserCount = 5, AdminCount = 3 });
            }
            else if (roleId == "9FCBF75F-754E-4D47-A9F8-67C535B4FF25")
            {
                mockService.Setup(service => service.UserManagementByRoleid(Roleid))
                    .ReturnsAsync(new AdminUserDTO
                    {
                        Id = 1,
                        UserId = Guid.NewGuid(),
                        DepartmentId = Guid.NewGuid(),
                        DepartmentName = "Service",
                        Username = "User",
                        Email = "dummyuser@example.com",
                        ProfilePicture = new byte[] { 0x01, 0x02, 0x03 },
                        Status = true
                    });
            }

            var controller = new UserManagementController(mockService.Object);

            // Act
            var result = await controller.UserManagementByRoleid(Roleid);
            // Assert
            if (roleId == "7F89D12F-2383-42FF-9B26-63132C92A7A8")
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var counts = Assert.IsType<UserCounts>(okResult.Value);
                Assert.Equal(5, counts.UserCount);
                Assert.Equal(3, counts.AdminCount);
            }
            else if (roleId == "9FCBF75F-754E-4D47-A9F8-67C535B4FF25")
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var userDetails = Assert.IsType<AdminUserDTO>(okResult.Value);
                Assert.Equal(1, userDetails.Id);
            }
        }

        [Fact]
        public async Task UserManagementByRoleid_NullRoleId_ReturnsNotFound()
        {
            Guid? adminRoleId = null;

            var mockService = new Mock<IUserManagementService>();
            mockService.Setup(service => service.UserManagementByRoleid(It.IsAny<Guid>()))
                .ReturnsAsync(null);

            var controller = new UserManagementController(mockService.Object);

            // Act
            var result = await controller.UserManagementByRoleid(adminRoleId);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
