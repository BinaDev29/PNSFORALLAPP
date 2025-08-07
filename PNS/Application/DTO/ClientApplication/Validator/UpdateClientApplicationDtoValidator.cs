using FluentValidation;
using Application.DTO.ClientApplication;

namespace Application.DTO.ClientApplication.Validator
{
    public class UpdateClientApplicationDtoValidator : AbstractValidator<UpdateClientApplicationDto>
    {
        public UpdateClientApplicationDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}