using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.Priority;

namespace Application.DTO.Priority.Validator
{
    public class UpdatePriorityDtoValidator : AbstractValidator<UpdatePriorityDto>
    {
        public UpdatePriorityDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}