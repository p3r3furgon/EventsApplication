using FluentValidation;
using MediatR;
using Users.Application.CommonValidation;

namespace Users.Application.UseCases.UserUseCases.Commands.UpdateUser
{
    public record UpdateUserCommand(Guid Id, string? FirstName, string? Surname, string? BirthDate, string? Email, string? Password) : IRequest<UpdateUserResponse>;
    
    public record UpdateUserResponse(Guid Id);

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
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
                .Length(4, 32).WithMessage("Password must containes from 4 to 32 symbols")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Password must contain only letters and numbers")
                .When(request => !string.IsNullOrEmpty(request.Password));

            RuleFor(request => request.BirthDate)
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Birth date must be in format yyyy-MM-dd")
                .Must(SharedCheck.BeAValidPastDate).WithMessage("Birth date must be a valid date in the past")
                .Must(SharedCheck.BeValidYear).WithMessage("Year must be between 1900 and the current year")
                .When(request => !string.IsNullOrEmpty(request.BirthDate));


        }
    }
}
