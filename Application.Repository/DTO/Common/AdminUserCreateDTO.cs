using Microsoft.AspNetCore.Http;

namespace Application.Repository.DTO.Common
{
    public class AdminUserCreateDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IFormFile ProfilePicture { get; set; }

        public bool Status { get; set; }

        public Guid RoleId { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
