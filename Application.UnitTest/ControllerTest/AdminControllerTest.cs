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
            UserUpdateDTO userUpdateDTO = null;
            string output = null;

            var existingUserId = Guid.Parse("093EC28D-3900-4556-BAAE-36D4D251DAEA"); 
            var mockRepository = new Mock<IAdminRepository>();
            mockRepository.Setup(repo => repo.UpdateUser(existingUserId, userUpdateDTO))
                .ReturnsAsync(output);
            var adminService = new AdminService(mockRepository.Object);


            var result = await adminService.UpdateUser(existingUserId, userUpdateDTO);
            Assert.Equal(output, result);
        }
    }
}
