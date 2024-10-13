using FluentValidation;
using Users.API.Dtos;

namespace Users.API.Validators
{
    public class UpdateUserRequestValidator: AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(request => request.FirstName)
                 .Length(1, 100).WithMessage("First name length must be between 1 and 50")
                 .Matches("^[a-zA-Z]*$").WithMessage("First name format is incorrect")
                 .When(request => !string.IsNullOrEmpty(request.FirstName));

            RuleFor(request => request.Surname)
                 .Length(1, 100).WithMessage("surname length must be between 1 and 50")
                 .Matches("^[a-zA-Z]*$").WithMessage("Surname format is incorrect")
                 .When(request => !string.IsNullOrEmpty(request.Surname));

            RuleFor(request => request.Email)
                .EmailAddress().WithMessage("Email format is incorrect")
                .When(request => !string.IsNullOrEmpty(request.Email));

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers")
                .When(request => !string.IsNullOrEmpty(request.Password));
        }
    }
}
