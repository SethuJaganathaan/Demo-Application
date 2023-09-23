using Application.Repository.DTO.Admin;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using Application.Service.Interfaces;

namespace Application.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<string> DeleteUser(Guid userId)
        {
            return await _adminRepository.DeleteUser(userId);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _adminRepository.GetUserById(userId);
        }

        public async Task<string> UpdateUser(Guid userId, UserUpdateDTO user)
        {
            return await _adminRepository.UpdateUser(userId, user);
        }
    }
}
