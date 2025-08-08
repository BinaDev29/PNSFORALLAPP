using FluentValidation;
using Application.DTO.ClientApplication;

namespace Application.DTO.ClientApplication.Validator
{
    public class UpdateClientApplicationDtoValidator : AbstractValidator<UpdateClientApplicationDto>
    {
        public UpdateClientApplicationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}