using Application.Repository.DTO.User;
using Application.Repository.Enums;

namespace Application.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllUsers();

        Task<UserDTO> GetUserById(Guid userId);

        Task<bool> SoftDeleteUser(Guid userId);
    }
}
