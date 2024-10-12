using Events.API.Dtos;
using FluentValidation;

namespace Events.API.Validators
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(request => request.Title).Length(3, 100).WithMessage("Event title length must be between 3 and 100");
            RuleFor(request => request.Category).Length(3,100).WithMessage("Event category name length must be between 3 and 100");
            RuleFor(request => request.Description).Length(1, 1000).WithMessage("Event description length must be between 1 and 1000");
            RuleFor(request => request.Place).Length(1, 100).WithMessage("Event place name length must be between 1 and 100");
            RuleFor(request => request.MaxparticipantNumber).InclusiveBetween(1, 1000000).WithMessage("Event maxParticipants number must be between 1 and 1000000");
            RuleFor(request => request.DateTime).GreaterThan(DateTime.Now).WithMessage("Event date and time must be set correctly");

        }
    }
}
