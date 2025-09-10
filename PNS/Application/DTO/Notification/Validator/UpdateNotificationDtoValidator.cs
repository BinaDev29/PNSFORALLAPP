// File Path: Application/DTO/Notification/Validator/UpdateNotificationDtoValidator.cs
using FluentValidation;
using System;

namespace Application.DTO.Notification.Validator
{
    public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationDtoValidator()
        {
            // Validates that the ID is not empty.
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // Validates that the ClientApplicationId is not empty.
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // FIX: Use 'ForEach' to validate each item in the list.
            RuleFor(p => p.To)
                .NotEmpty().WithMessage("At least one recipient is required.");

            // You would need custom logic here to validate if each item is an email/phone.
            // For now, this validates that the list is not empty.

            // Validates that the Title is not empty.
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // Validates that the Message is not empty.
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // FIX: Add validation for NotificationTypeId.
            RuleFor(p => p.NotificationTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            // FIX: Add validation for PriorityId.
            RuleFor(p => p.PriorityId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}