using FluentValidation;
using MediatR;
using Users.Application.CommonValidations;
using Users.Application.Dtos;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserRegister
{
    public record UserRegisterCommand(UserRequestDto UserDto) : IRequest<UserRegisterResponse>;

    public record UserRegisterResponse(Guid Id);

    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterCommandValidator()
        {
            RuleFor(request => request.UserDto.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(1, 50).WithMessage("First name length must be between 1 and 100");

            RuleFor(request => request.UserDto.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(1, 50).WithMessage("Surname length must be between 1 and 100");

            RuleFor(request => request.UserDto.Email).NotEmpty().EmailAddress().WithMessage("Email format is incorrect");

            RuleFor(request => request.UserDto.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Birth date must be in format yyyy-MM-dd")
                .Must(SharedCheck.BeAValidPastDate).WithMessage("Birth date must be a valid date in the past")
                .Must(SharedCheck.BeValidYear).WithMessage("Year must be between 1900 and the current year");

            RuleFor(request => request.UserDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers");
        }
    }

}
