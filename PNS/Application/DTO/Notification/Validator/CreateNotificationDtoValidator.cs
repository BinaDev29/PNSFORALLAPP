using FluentValidation;
using Application.DTO.Notification;

namespace Application.DTO.Notification.Validator
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.NotificationTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Recipient)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}