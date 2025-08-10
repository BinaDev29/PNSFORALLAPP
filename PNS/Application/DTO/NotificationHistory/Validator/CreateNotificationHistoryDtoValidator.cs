// File Path: Application/DTO/NotificationHistory/Validator/CreateNotificationHistoryDtoValidator.cs
using FluentValidation;

namespace Application.DTO.NotificationHistory.Validator
{
    public class CreateNotificationHistoryDtoValidator : AbstractValidator<CreateNotificationHistoryDto>
    {
        public CreateNotificationHistoryDtoValidator()
        {
            RuleFor(p => p.NotificationId).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}