// File Path: Application/DTO/Notification/Validator/UpdateNotificationDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.Notification.Validator
{
    public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.To)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} must be a valid email address.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}