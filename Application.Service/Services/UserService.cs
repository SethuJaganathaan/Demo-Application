using Application.Repository.DTO.User;
using Application.Repository.Enums;
using Application.Repository.Interfaces;
using Application.Repository.Repositories;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Application.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<bool> SoftDeleteUser(Guid userId)
        {
            return await _userRepository.SoftDeleteUser(userId);
        }
    }
}
