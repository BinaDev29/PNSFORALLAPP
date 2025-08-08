using FluentValidation;
using Application.DTO.Priority;

namespace Application.DTO.Priority.Validator
{
    public class UpdatePriorityDtoValidator : AbstractValidator<UpdatePriorityDto>
    {
        public UpdatePriorityDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Level)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be at least 1.");
        }
    }
}