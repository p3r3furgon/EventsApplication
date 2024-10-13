using Events.API.Dtos;
using FluentValidation;

namespace Events.API.Validators
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(request => request.Title).Length(3, 100)
                 .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event title length must be between 3 and 100");
            RuleFor(request => request.Category).Length(3,100)
                .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event category name length must be between 3 and 100");
            RuleFor(request => request.Description).Length(1, 1000)
                .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event description length must be between 1 and 1000");
            RuleFor(request => request.Place).Length(1, 100)
                .When(request => !string.IsNullOrEmpty(request.Category)).WithMessage("Event place name length must be between 1 and 100");
            RuleFor(request => request.MaxparticipantNumber).InclusiveBetween(1, 1000000)
                .When(request => request.MaxparticipantNumber != null).WithMessage("Event maxParticipants number must be between 1 and 1000000");
            RuleFor(request => request.DateTime).GreaterThan(DateTime.Now)
                .When(request => request.MaxparticipantNumber != null).WithMessage("Event date and time must be set correctly");
            RuleFor(request => request.MessageTitle).NotEmpty()
            .When(request => !string.IsNullOrEmpty(request.MessageDescription)).WithMessage("If you want to publish message you need to fill both title and description");
                        RuleFor(request => request.MessageDescription).NotEmpty()
            .When(request => !string.IsNullOrEmpty(request.MessageTitle)).WithMessage("If you want to publish message you need to fill both title and description");
        }
    }
}
