// File Path: Application/DTO/EmailTemplate/Validator/CreateEmailTemplateDtoValidator.cs
using FluentValidation;

namespace Application.DTO.EmailTemplate.Validator
{
    public class CreateEmailTemplateDtoValidator : AbstractValidator<CreateEmailTemplateDto>
    {
        public CreateEmailTemplateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Subject).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.BodyHtml).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}