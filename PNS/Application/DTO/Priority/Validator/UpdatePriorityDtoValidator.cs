// File Path: Application/DTO/Priority/Validator/UpdatePriorityDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.Priority.Validator
{
    public class UpdatePriorityDtoValidator : AbstractValidator<UpdatePriorityDto>
    {
        public UpdatePriorityDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Level)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}