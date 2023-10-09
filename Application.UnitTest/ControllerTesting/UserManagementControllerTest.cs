using Application.API.Controllers;
using Application.Repository.DTO.Common;
using Application.Repository.Entities;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class UserManagementControllerTest
    {
        [Fact]
        public async Task UserManagementRoleId_NormalUser_ReturnOkResultWithCounts()
        {
            // Arrange
            var normalUserRoleId = Guid.Parse("7F89D12F-2383-42FF-9B26-63132C92A7A8");
            var mockService = new Mock<IUserManagementService>();
            mockService.Setup(set => set.UserManagementByRoleid(normalUserRoleId))
                .ReturnsAsync(new
                {
                    UserCounts = 5,
                    AdminCounts = 3
                });

            var controller = new UserManagementController(mockService.Object);

            // Act
            var result = await controller.UserManagementByRoleid(normalUserRoleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            var userCounts = response.GetType().GetProperty("UserCounts")?.GetValue(response);
            var adminCounts = response.GetType().GetProperty("AdminCounts")?.GetValue(response);
            Assert.Equal(5, userCounts);
            Assert.Equal(3, adminCounts);
        }

        [Fact]
        public async Task UserManagementRoleId_SuperAdmin_ReturnOkResultWithDetails()
        {
            var superAdmin = Guid.Parse("9FCBF75F-754E-4D47-A9F8-67C535B4FF25");
            var mockservice = new Mock<IUserManagementService>();

            var userDetailList = new List<AdminUserDTO>()
            {
                new AdminUserDTO
                {
                    Id = 1,
                    UserId = new Guid("73F61A2B-BC62-4FDF-939D-F352DB5616F7"),
                    DepartmentId = new Guid("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"),
                    DepartmentName = "Service",
                    Username = "Sanji",
                    Email = "sanji@gmail.com",
                    ProfilePicture = new byte[] { 1, 3, 4, 5, 6 },
                    Status = 2
                },
                new AdminUserDTO
                {
                    Id = 2,
                    UserId = new Guid("BB49C349-5305-4609-957A-F070E421A213"),
                    DepartmentId = new Guid("C38F7FFB-7960-42BF-9107-16AAB1503F9E"),
                    DepartmentName = "Product",
                    Username = "Nami san",
                    Email = "nami@gmail.com",
                    ProfilePicture = new byte[] { 1, 3, 4, 3, 6 },
                    Status = 1
                }
            };

            mockservice.Setup(set => set.UserManagementByRoleid(superAdmin))
                .ReturnsAsync(userDetailList) ;

            var controller = new UserManagementController(mockservice.Object);

            // Act
            var result = await controller.UserManagementByRoleid(superAdmin);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as List<AdminUserDTO>;
            Assert.Equal(1, response[0].Id);
            Assert.Equal(new Guid("73F61A2B-BC62-4FDF-939D-F352DB5616F7"), response[0].UserId);
            Assert.Equal(new Guid("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"), response[0].DepartmentId);
            Assert.Equal("Service", response[0].DepartmentName);
            Assert.Equal("Sanji", response[0].Username);
            Assert.Equal("sanji@gmail.com", response[0].Email);
            Assert.Equal(new byte[] { 1, 3, 4, 5, 6 }, response[0].ProfilePicture);
            Assert.Equal(2, response[0].Status);
        }

        [Fact]
        public async Task UserManagementRoleId_InvalidGuid()
        {
            // Arrange
            var invalidGuid = Guid.NewGuid();
            var mockservice = new Mock<IUserManagementService>();
            var controller = new UserManagementController(mockservice.Object);

            // Act
            var result = await controller.UserManagementByRoleid(invalidGuid) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, result.StatusCode);
        }

    }
}
