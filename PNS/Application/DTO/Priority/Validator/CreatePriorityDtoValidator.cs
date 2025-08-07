using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.Priority;

namespace Application.DTO.Priority.Validator
{
    public class CreatePriorityDtoValidator : AbstractValidator<CreatePriorityDto>
    {
        public CreatePriorityDtoValidator()
        {
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Level)
                .GreaterThanOrEqualTo(1).WithMessage("{PropertyName} must be at least 1.");
        }
    }
}