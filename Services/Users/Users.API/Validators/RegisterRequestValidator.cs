using FluentValidation;
using Users.API.Dtos;

namespace Users.API.Validators
{
    public class RegisterRequestValidator: AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(1, 50).WithMessage("First name length must be between 1 and 100");

            RuleFor(request => request.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(1, 50).WithMessage("Surname length must be between 1 and 100");

            RuleFor(request => request.Email).NotEmpty().EmailAddress().WithMessage("Email format is incorrect");

            RuleFor(request => request.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Incorrect birthdate data");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers");
        }
    }
}
