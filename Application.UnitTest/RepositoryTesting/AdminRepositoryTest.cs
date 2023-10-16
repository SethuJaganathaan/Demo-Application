using Application.Repository.Context;
using Application.Repository.Core;
using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.UnitTest.RepositoryTesting
{
    public class AdminRepositoryTest
    {
        private IMapper _mapper;
        public AdminRepositoryTest()
        {
            var configuration = new MapperConfiguration(core =>
            {
                core.AddProfile<MappingProfiles>(); 
            });

            _mapper = new Mapper(configuration);
        }

        [Fact]
        public async Task DeleteUser_ValidUserId_ReturnActionSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            using (var dbContext = new ApplicationContext(options))
            {
                dbContext.Users.Add(new User
                {
                    UserId = userId,
                    Email = "test@example.com",
                    Password = "password",
                    ProfilePicture = new byte[] {1, 2, 3, 4},
                    Username = "testuser"
                });
                dbContext.SaveChanges();
            }

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.DeleteUser(userId);

            // Assert
            Assert.Equal("Action Successful", result);
        }

        [Fact]
        public async Task DeleteUser_InvalidUserId_ReturnNotFound()
        {
            // Arrange
            var userId = Guid.Empty;
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.DeleteUser(userId);

            // Assert
            Assert.Equal("Invalid ID", result);
        }

        [Fact]
        public async Task DeleteUser_NonexistentUser_ReturnUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.DeleteUser(userId);

            // Assert
            Assert.Equal("User not found", result);
        }

        [Fact]
        public async Task UpdateUser_WithValidUserId_ReturnActionSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "profile.jpg");
            var userUpdateDTO = new UserUpdateDTO
            {
                Username = "Sanji",
                Email = "sanji@gmail.com",
                Password = "Sanji@123",
                RoleId = Guid.Parse("7F89D12F-2383-42FF-9B26-63132C92A7A8"),
                DepartmentId = Guid.Parse("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"),
                ProfilePicture = profilePicture
            };

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationContext(options))
            {
                dbContext.Users.Add(new User
                {
                    UserId = userId,
                    Email = "zoro@example.com",
                    Password = "Zoro123",
                    ProfilePicture = new byte[] { 1, 2, 3, 4 },
                    Username = "Zoro"
                });
                dbContext.SaveChanges();
            }

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.UpdateUser(userId, userUpdateDTO);

            // Assert
            Assert.Equal("Action Successful", result);
        }

        [Fact]
        public async Task UpdateUser_InvalidUserId_ReturnNotFound()
        {
            // Arrange
            var userId = Guid.Empty;
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "profile.jpg");
            var userUpdateDTO = new UserUpdateDTO
            {
                Username = "Sanji",
                Email = "sanji@gmail.com",
                Password = "sgdfytgsv",
                RoleId = Guid.Parse("7F89D12F-2383-42FF-9B26-63132C92A7A8"),
                DepartmentId = Guid.Parse("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"),
                ProfilePicture = profilePicture,
            };

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.UpdateUser(userId, userUpdateDTO);

            // Assert
            Assert.Equal("Invalid ID", result);
        }

        [Fact]
        public async Task UpdateUser_NonexistentUser_ReturnNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profilePicture = new FormFile(new MemoryStream(new byte[] { 1, 2, 9, 10 }), 0, 4, "ProfilePicture", "profile.jpg");
            var userUpdateDTO = new UserUpdateDTO
            {
                Username = "Sanji",
                Email = "sanji@gmail.com",
                Password = "sgdfytgsv",
                RoleId = Guid.Parse("7F89D12F-2383-42FF-9B26-63132C92A7A8"),
                DepartmentId = Guid.Parse("113D2E85-E0E6-44B6-85C9-D6059D6B97B6"),
                ProfilePicture = profilePicture,
            };

            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.UpdateUser(userId, userUpdateDTO);

            // Assert
            Assert.Equal("Not Found", result);
        }

        [Fact]
        public async Task GetUserById_NonexistentUser_ReturnUserDTOWithNotFoundMessage()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "Database")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User not found", result.message);
        }

        [Fact]
        public async Task GetUserById_InvalidUserId_ReturnUserDTOWithNotFoundMessage()
        {
            // Arrange
            var userId = Guid.Empty;
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var mapper = new Mock<IMapper>();
            var adminRepository = new AdminRepository(new ApplicationContext(options), mapper.Object);

            // Act
            var result = await adminRepository.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid ID", result.message);
        }
    }
}
