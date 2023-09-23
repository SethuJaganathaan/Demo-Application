﻿using Application.Repository.DTO.Common;
using Application.Service.Interfaces;
using Microsoft.AspNetCore.Http;
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
    }
}