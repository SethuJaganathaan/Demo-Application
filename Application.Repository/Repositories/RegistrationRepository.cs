using Application.Repository.Context;
using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;

namespace Application.Repository.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;

        string result = "Action Successful";
        public RegistrationRepository(ApplicationContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<string> SignUp(AdminUserCreateDTO user)
        {
            if (await _dbcontext.Users.AnyAsync(u => u.Email == user.Email))
                return "Email already exist";

            if (user.ProfilePicture != null)
            {
                string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(user.ProfilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return "Invalid picture format";
            }

            var newUser = _mapper.Map<User>(user);
            using (var memoryStream = new MemoryStream())
            {
                await user.ProfilePicture.CopyToAsync(memoryStream);
                newUser.ProfilePicture = memoryStream.ToArray();
            }

            _dbcontext.Users.Add(newUser);
            await _dbcontext.SaveChangesAsync();
            return result;
        }
    }
}
