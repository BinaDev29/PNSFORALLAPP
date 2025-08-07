using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.EmailTemplate;

namespace Application.DTO.EmailTemplate.Validator
{
    public class UpdateEmailTemplateDtoValidator : AbstractValidator<UpdateEmailTemplateDto>
    {
        public UpdateEmailTemplateDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Subject)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}