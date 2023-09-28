using Application.Repository.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Repository.DTO.User
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string RoleName { get; set; }

        public string DepartmentName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public byte[] ProfilePicture { get; set; }

        public short Status { get; set; }

        public string message { get; set; }
    }
}
