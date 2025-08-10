// File Path: Application/DTO/Priority/Validator/CreatePriorityDtoValidator.cs
using FluentValidation;

namespace Application.DTO.Priority.Validator
{
    public class CreatePriorityDtoValidator : AbstractValidator<CreatePriorityDto>
    {
        public CreatePriorityDtoValidator()
        {
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Level)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}