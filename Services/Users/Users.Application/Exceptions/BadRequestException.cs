using FluentValidation.Results;

namespace Users.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<ValidationFailure> Errors { get; } = new List<ValidationFailure>();

        public BadRequestException(List<ValidationFailure> failures)
            : base("Validation failed")
        {
            Errors = failures;
        }

        public BadRequestException(string message)
            : base(message){}

        public override string ToString()
        {
            var errorMessages = Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
            return $"{Message}: {string.Join("; ", errorMessages)}";
        }
    }
}
