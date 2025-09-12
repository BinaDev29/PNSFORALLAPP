// File Path: Application/DTO/Notification/Validator/CreateNotificationDtoValidator.cs
using FluentValidation;

namespace Application.DTO.Notification.Validator
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleForEach(p => p.To)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("Each {PropertyName} must be a valid email address.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}