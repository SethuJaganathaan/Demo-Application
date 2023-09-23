using Application.Repository.DTO.Common;

namespace Application.Repository.Interfaces
{
    public interface IUserManagementRepository
    {
        Task<object> UserManagementByRoleid(Guid roleid);
    }
}
