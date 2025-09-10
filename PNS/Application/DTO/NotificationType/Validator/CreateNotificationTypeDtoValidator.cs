// File Path: Application/DTO/NotificationType/Validator/CreateNotificationTypeDtoValidator.cs
using FluentValidation;

namespace Application.DTO.NotificationType.Validator
{
    public class CreateNotificationTypeDtoValidator : AbstractValidator<CreateNotificationTypeDto>
    {
        public CreateNotificationTypeDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}