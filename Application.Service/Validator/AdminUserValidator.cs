using Application.Repository.DTO.Common;
using FluentValidation;

namespace Application.Service.Validator
{
    public class AdminUserValidator : AbstractValidator<AdminUserCreateDTO>
    {
        public AdminUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(5).WithMessage("Username minimum length of 5 Character")
                .MaximumLength(255).WithMessage("Username must not exceed 20 characters");

            RuleFor(x => x.Email).EmailAddress()
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
                .WithMessage("Password must contain at least one alphabet character, one number, and one special character.");

            RuleFor(x => x.ProfilePicture)
                .NotNull().WithMessage("Profilepicture is required");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("DepartmentId is required");
        }
    }
}
