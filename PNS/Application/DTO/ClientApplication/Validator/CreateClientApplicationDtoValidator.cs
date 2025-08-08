using FluentValidation;
using Application.DTO.ClientApplication;

namespace Application.DTO.ClientApplication.Validator
{
    public class CreateClientApplicationDtoValidator : AbstractValidator<CreateClientApplicationDto>
    {
        public CreateClientApplicationDtoValidator()
        {
            RuleFor(p => p.AppId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}