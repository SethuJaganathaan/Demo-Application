using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Interfaces;

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
    }
}
