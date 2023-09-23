namespace Application.Service.Interfaces
{
    public interface IUserManagementService
    {
        Task<object> UserManagementByRoleid(Guid roleid);
    }
}
