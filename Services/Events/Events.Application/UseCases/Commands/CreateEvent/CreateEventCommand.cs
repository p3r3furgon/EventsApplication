using Events.Application.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Events.Application.UseCases.Commands.CreateEvent
{
    public record CreateEventCommand(string Title, string? Description, string Place, 
        DateTime DateTime, string? Category, int MaxParticipantNumber, IFormFile? Image) 
        : IRequest<CreateEventResponse>;
    public record CreateEventResponse(Guid Id);

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("Event must has title")
                .Length(3, 100).WithMessage("Event title length must be between 3 and 100");

            RuleFor(request => request.Place)
                .NotEmpty().WithMessage("Event place must be mentioned")
                .Length(1, 100).WithMessage("Event place name length must be between 1 and 100");

            RuleFor(request => request.MaxParticipantNumber)
                .InclusiveBetween(1, 1000000).WithMessage("Event maxParticipants number must be between 1 and 1000000");

            RuleFor(request => request.DateTime)
                .NotEmpty().WithMessage("Event date and time must be mentioned")
                .GreaterThan(DateTime.Now).WithMessage("Event date and time must be set correctly");

            RuleFor(request => request.Category).Length(3, 100)
                .When(request => !string.IsNullOrEmpty(request.Category))
                .WithMessage("Event category name length must be between 3 and 100");

            RuleFor(request => request.Description).Length(1, 1000)
                .When(request => !string.IsNullOrEmpty(request.Category))
                .WithMessage("Event description length must be between 1 and 1000");
        }
    }

}
