using Application.Repository.DTO.Admin;
using Application.Repository.Entities;

namespace Application.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<User> GetUserById(Guid userId);

        Task<string> UpdateUser(Guid userId, UserUpdateDTO user);

        Task<string> DeleteUser(Guid userId);

    }
}
