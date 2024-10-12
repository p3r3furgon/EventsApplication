using Events.API.Dtos;
using FluentValidation;

namespace Events.API.Validators
{
    public class CreateEventRequestValidator: AbstractValidator<CreateEventRequest>
    {
        public CreateEventRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("Event must has title")
                .Length(3,100).WithMessage("Event title length must be between 3 and 100");

            RuleFor(request => request.Place)
                .NotEmpty().WithMessage("Event place must be mentioned")
                .Length(1, 100).WithMessage("Event place name length must be between 1 and 100");

            RuleFor(request => request.MaxparticipantNumber)
                .InclusiveBetween(1, 1000000).WithMessage("Event maxParticipants number must be between 1 and 1000000");

            RuleFor(request => request.DateTime)
                .GreaterThan(DateTime.Now).WithMessage("Event date and time must be set correctly");

            RuleFor(request => request.Category).Length(3, 100).
                When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event category name length must be between 3 and 100");

            RuleFor(request => request.Description).Length(1, 1000).
                When(request => !string.IsNullOrEmpty(request.Description)).WithMessage("Event description length must be between 1 and 1000");
        }
    }
}
