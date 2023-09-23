using Application.Repository.Context;
using Application.Repository.DTO.Admin;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;
        string result = "Action Successful";
        public AdminRepository(ApplicationContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<string> DeleteUser(Guid userId)
        {
            var user = await GetUserById(userId);
            _dbcontext.Users.Remove(user);
            await _dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _dbcontext.Users.FindAsync(userId);
        }

        public async Task<string> UpdateUser(Guid userId, UserUpdateDTO user)
        {
            var existingUser = await _dbcontext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingUser == null)
            {
                return "Not Found";
            }

            _mapper.Map(user, existingUser);
            await _dbcontext.SaveChangesAsync();
            return result;
        }
    }
}
