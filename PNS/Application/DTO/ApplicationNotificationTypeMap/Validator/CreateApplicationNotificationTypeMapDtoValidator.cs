// File Path: Application/DTO/ApplicationNotificationTypeMap/Validator/CreateApplicationNotificationTypeMapDtoValidator.cs
using FluentValidation;

namespace Application.DTO.ApplicationNotificationTypeMap.Validator
{
    public class CreateApplicationNotificationTypeMapDtoValidator : AbstractValidator<CreateApplicationNotificationTypeMapDto>
    {
        public CreateApplicationNotificationTypeMapDtoValidator()
        {
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.NotificationTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}