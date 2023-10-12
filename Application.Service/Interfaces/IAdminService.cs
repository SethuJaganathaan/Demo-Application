using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;

namespace Application.Service.Interfaces
{
    public interface IAdminService
    {
        Task<UserDTO> GetUserById(Guid userId);

        Task<string> DeleteUser(Guid userId);

        Task<string> UpdateUser(Guid userId, UserUpdateDTO user);
    }
}
