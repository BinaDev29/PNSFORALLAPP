using FluentValidation;
using Application.DTO.NotificationHistory;

namespace Application.DTO.NotificationHistory.Validator
{
    public class CreateNotificationHistoryDtoValidator : AbstractValidator<CreateNotificationHistoryDto>
    {
        public CreateNotificationHistoryDtoValidator()
        {
            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}