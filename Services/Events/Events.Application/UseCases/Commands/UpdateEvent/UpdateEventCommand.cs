using Events.Application.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Events.Application.UseCases.Commands.UpdateEvent
{
    public record UpdateEventCommand(Guid Id, string? Title, string? Description, string? Place,
        DateTime? DateTime, string? Category, int? MaxParticipantNumber, IFormFile? Image, string? MessageTitle, string? MessageContent) 
        : IRequest<UpdateEventResponse>;
    public record UpdateEventResponse(Guid Id);

    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(request => request.Title).Length(3, 100)
                 .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event title length must be between 3 and 100");

            RuleFor(request => request.Category).Length(3, 100)
                .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event category name length must be between 3 and 100");

            RuleFor(request => request.Description).Length(1, 1000)
                .When(request => !string.IsNullOrEmpty(request.Description)).WithMessage("Event description length must be between 1 and 1000");

            RuleFor(request => request.Place).Length(1, 100)
                .When(request => !string.IsNullOrEmpty(request.Place)).WithMessage("Event place name length must be between 1 and 100");

            RuleFor(request => request.MaxParticipantNumber).InclusiveBetween(1, 1000000)
                .When(request => request.MaxParticipantNumber != null).WithMessage("Event maxParticipants number must be between 1 and 1000000");

            RuleFor(request => request.DateTime).GreaterThan(DateTime.Now)
                .When(request => request.DateTime != null).WithMessage("Event date and time must be set correctly");

            RuleFor(request => request.MessageTitle).NotEmpty()
                .When(request => !string.IsNullOrEmpty(request.MessageContent)).WithMessage("If you want to publish message you need to fill both title and description");

            RuleFor(request => request.MessageContent).NotEmpty()
                .When(request => !string.IsNullOrEmpty(request.MessageTitle)).WithMessage("If you want to publish message you need to fill both title and description");

        }
    }
}
