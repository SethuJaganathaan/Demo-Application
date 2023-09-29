using Application.Repository.Context;
using Application.Repository.DTO.Common;
using Application.Repository.Interfaces;
using Application.Service.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            if (Roleid == Guid.Empty)
            {
                return new { Message = "Invalid Roleid" };
            }

            var superAdminRole = await _dbcontext.Roles
                .FirstOrDefaultAsync(r => r.Rolename == CommonConstant.Role.Admin);

            if (superAdminRole == null)
            {
                return new { Message = "SuperAdmin role not found" };
            }

            bool isSuperAdmin = Roleid == superAdminRole.RoleId;

            if (isSuperAdmin)
            {
                var superAdminUsers = await (from Roles in _dbcontext.Roles
                                             join users in _dbcontext.Users on Roles.RoleId equals users.RoleId
                                             select new AdminUserDTO
                                             {
                                                 UserId = users.UserId,
                                                 DepartmentId = users.DepartmentId ?? Guid.Empty,
                                                 Username = users.Username,
                                                 Email = users.Email,
                                                 ProfilePicture = users.ProfilePicture,
                                             }).ToListAsync();

                return superAdminUsers; 
            }

            return new
            {
                UserCount = await _dbcontext.Users.CountAsync(u => u.RoleId == Roleid),
                AdminCount = await _dbcontext.Users.CountAsync(u => u.RoleId == superAdminRole.RoleId)
            };
        }

    }
}
