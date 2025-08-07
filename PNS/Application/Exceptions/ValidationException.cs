using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Application.Exceptions
{
    // ከሌሎቹ Exceptions ጋር እንዲመሳሰል ከ ApplicationException ይወርሳል
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}