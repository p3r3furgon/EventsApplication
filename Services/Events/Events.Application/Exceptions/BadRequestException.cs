﻿using FluentValidation.Results;

namespace Events.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<ValidationFailure> Errors { get; }

        public BadRequestException(List<ValidationFailure> failures)
            : base("Validation failed")
        {
            Errors = failures;
        }

        public override string ToString()
        {
            var errorMessages = Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
            return $"{Message}: {string.Join("; ", errorMessages)}";
        }
    }
}
