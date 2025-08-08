using FluentValidation;
using Application.DTO.EmailTemplate;

namespace Application.DTO.EmailTemplate.Validator
{
    public class UpdateEmailTemplateDtoValidator : AbstractValidator<UpdateEmailTemplateDto>
    {
        public UpdateEmailTemplateDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Subject)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.BodyHtml)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}