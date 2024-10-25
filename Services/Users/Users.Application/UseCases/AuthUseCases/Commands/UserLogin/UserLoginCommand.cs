using FluentValidation;
using MediatR;

namespace Users.Application.UseCases.AuthUseCases.Commands.UserLogin
{
    public record UserLoginCommand(string Email, string Password) : IRequest<UserLoginResponse>;

    public record UserLoginResponse(string AccessToken, string RefreshToken);

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
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
