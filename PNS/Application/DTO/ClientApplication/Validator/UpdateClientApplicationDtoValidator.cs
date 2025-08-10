// File Path: Application/DTO/ClientApplication/Validator/UpdateClientApplicationDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.ClientApplication.Validator
{
    public class UpdateClientApplicationDtoValidator : AbstractValidator<UpdateClientApplicationDto>
    {
        public UpdateClientApplicationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.AppId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}