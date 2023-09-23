using Application.Repository.DTO.Admin;
using Application.Repository.Entities;

namespace Application.Service.Interfaces
{
    public interface IAdminService
    {
        Task<User> GetUserById(Guid userId);

        Task<string> DeleteUser(Guid userId);

        Task<string> UpdateUser(Guid userId, UserUpdateDTO user);
    }
}
