// File Path: Application/DTO/NotificationType/Validator/UpdateNotificationTypeDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.NotificationType.Validator
{
    public class UpdateNotificationTypeDtoValidator : AbstractValidator<UpdateNotificationTypeDto>
    {
        public UpdateNotificationTypeDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}