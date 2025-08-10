// File Path: Application/DTO/EmailTemplate/Validator/UpdateEmailTemplateDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.EmailTemplate.Validator
{
    public class UpdateEmailTemplateDtoValidator : AbstractValidator<UpdateEmailTemplateDto>
    {
        public UpdateEmailTemplateDtoValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Subject).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.BodyHtml).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}