using FluentValidation;
using Users.API.Dtos;

namespace Users.API.Validators
{
    public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Email format is incorrect");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers");
        }
    }
}
