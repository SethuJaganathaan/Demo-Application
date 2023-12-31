﻿using Application.Repository.DTO.Common;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp([FromForm] AdminUserCreateDTO user)
        {
            var newUser = await _registrationService.SignUp(user);
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var signIn = await _registrationService.Login(login); 
            return Ok(signIn);
        }


        [HttpGet("email")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _registrationService.GetUser(email);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }
    }
}
