using FluentValidation;
using Application.DTO.Notification;

namespace Application.DTO.Notification.Validator
{
    public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}