using Application.Repository.DTO.Admin;
using Application.Repository.Interfaces;
using Application.Service.Services;
using Moq;

namespace Application.UnitTest.ControllerTest
{
    public class AdminControllerTest
    {
        [Fact]
        public async Task UpdateUser_ReturnSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDTO
            {
                Username = "Dummy",
                Email = "dummy@gmail.com",
                Password = "dummy@*#5",
                ProfilePicture = null,
                RoleId = Guid.NewGuid(),
                DepartmentId = Guid.NewGuid()
            };

            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(userId, userUpdateDto))
                .ReturnsAsync("Action Successful");

            var adminService = new AdminService(mockRepository.Object);

            // Act
            var result = await adminService.UpdateUser(userId, userUpdateDto);

            // Assert
            Assert.Equal("Action Successful", result);
        }
    }
}
