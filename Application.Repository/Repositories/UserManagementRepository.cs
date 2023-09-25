using Application.Repository.Context;
using Application.Repository.DTO.Common;
using Application.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly ApplicationContext _dbcontext;
        public UserManagementRepository(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<object> UserManagementByRoleid(Guid Roleid)
        {
            bool isSuperAdmin = Roleid == new Guid("8783F704-4F57-42F1-879B-C056DF45CDD0");

            return isSuperAdmin
                ? await (from Roles in _dbcontext.Roles
                         join users in _dbcontext.Users on Roles.RoleId equals users.RoleId
                         select new AdminUserDTO
                         {
                             Id = users.Id,
                             UserId = users.UserId,
                             DepartmentId = users.DepartmentId ?? Guid.Empty,
                             Username = users.Username,
                             Email = users.Email,
                             ProfilePicture = users.ProfilePicture,
                             Status = users.Status ?? false,
                         }).ToListAsync()
                : new
                {
                    UserCount = await _dbcontext.Users.CountAsync(u => u.RoleId == Roleid),
                    SuperAdminCount = await _dbcontext.Users.CountAsync(u => u.RoleId == new Guid("8783F704-4F57-42F1-879B-C056DF45CDD0"))
                };
        }
    }
}
