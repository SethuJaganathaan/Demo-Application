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

            var result = await adminService.UpdateUser(userId, userUpdateDto);
            Assert.Equal("Action Successful", result);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDTO();
            var output = "Not Found";

            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(userId, userUpdateDto))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);

            var result = await adminService.UpdateUser(userId, userUpdateDto);
            Assert.Equal(output, result);
        }

        [Fact]
        public async Task UpdateUser_ReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            UserUpdateDTO userUpdateDTO = null;
            string output = null;

            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(userId, userUpdateDTO))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);

            var result = await adminService.UpdateUser(userId, userUpdateDTO);
            Assert.Equal(output, result);
        }
    }
}
