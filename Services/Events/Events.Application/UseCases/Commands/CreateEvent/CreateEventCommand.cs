using Events.Application.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Events.Application.UseCases.Commands.CreateEvent
{
    public record CreateEventCommand(EventRequestDto EventDto) 
        : IRequest<CreateEventResponse>;
    public record CreateEventResponse(Guid Id);

    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(request => request.EventDto.Title)
                .NotEmpty().WithMessage("Event must has title")
                .Length(3, 100).WithMessage("Event title length must be between 3 and 100");

            RuleFor(request => request.EventDto.Place)
                .NotEmpty().WithMessage("Event place must be mentioned")
                .Length(1, 100).WithMessage("Event place name length must be between 1 and 100");

            RuleFor(request => request.EventDto.MaxParticipantNumber)
                .InclusiveBetween(1, 1000000).WithMessage("Event maxParticipants number must be between 1 and 1000000");

            RuleFor(request => request.EventDto.DateTime)
                .NotEmpty().WithMessage("Event date and time must be mentioned")
                .GreaterThan(DateTime.Now).WithMessage("Event date and time must be set correctly");

            RuleFor(request => request.EventDto.Category).Length(3, 100)
                .When(request => !string.IsNullOrEmpty(request.EventDto.Category))
                .WithMessage("Event category name length must be between 3 and 100");

            RuleFor(request => request.EventDto.Description).Length(1, 1000)
                .When(request => !string.IsNullOrEmpty(request.EventDto.Category))
                .WithMessage("Event description length must be between 1 and 1000");
        }
    }

}
