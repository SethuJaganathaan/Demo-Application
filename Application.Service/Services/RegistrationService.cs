using Application.Repository.DTO.Common;
using Application.Repository.Interfaces;
using Application.Service.Interfaces;

namespace Application.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        public RegistrationService(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<BaseResponse> Login(LoginDTO login)
        {
            return await _registrationRepository.Login(login);
        }

        public async Task<string> SignUp(AdminUserCreateDTO user)
        {
            return await _registrationRepository.SignUp(user);
        }
    }
}
