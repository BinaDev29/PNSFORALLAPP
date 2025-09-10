// File Path: Application/DTO/NotificationType/Validator/UpdateNotificationTypeDtoValidator.cs
using Application.DTO.NotificationType;
using FluentValidation;

namespace Application.DTO.NotificationType.Validator
{
    public class UpdateNotificationTypeDtoValidator : AbstractValidator<UpdateNotificationTypeDto>
    {
        public UpdateNotificationTypeDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}