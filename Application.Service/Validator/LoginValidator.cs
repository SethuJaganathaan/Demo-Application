using Application.Repository.DTO.Common;
using FluentValidation;

namespace Application.Service.Validator
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.email).EmailAddress()
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid format");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
