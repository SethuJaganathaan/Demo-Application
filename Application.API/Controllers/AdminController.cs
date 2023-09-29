using Application.Repository.DTO.Admin;
using Application.Repository.Enums;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = CommonConstant.Policies.AdminPolicy)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("UserId")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _adminService.GetUserById(userId);
            return Ok(user);
        }

        [HttpPut("User")]
        public async Task<IActionResult> UpdateUser(Guid userId,[FromForm] UserUpdateDTO user)
        {
            var result = await _adminService.UpdateUser(userId, user);
            return Ok(result);
        }

        [HttpDelete("UserId")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _adminService.DeleteUser(userId);
            return Ok(result);
        }
    }
}
