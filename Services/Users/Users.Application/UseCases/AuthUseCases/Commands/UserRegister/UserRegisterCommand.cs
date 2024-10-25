using FluentValidation;
using MediatR;
using Users.Application.CommonValidation;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserRegister
{
    public record UserRegisterCommand(string FirstName, string Surname, string BirthDate, 
        string Email, string Password) : IRequest<UserRegisterResponse>;

    public record UserRegisterResponse(Guid Id);

    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterCommandValidator()
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
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Birth date must be in format yyyy-MM-dd")
                .Must(SharedCheck.BeAValidPastDate).WithMessage("Birth date must be a valid date in the past")
                .Must(SharedCheck.BeValidYear).WithMessage("Year must be between 1900 and the current year");

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers");
        }
    }

}
