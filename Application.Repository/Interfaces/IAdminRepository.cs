using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;

namespace Application.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<UserDTO> GetUserById(Guid userId);

        Task<string> UpdateUser(Guid userId, UserUpdateDTO user);

        Task<string> DeleteUser(Guid userId);

    }
}
