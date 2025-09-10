//-------------------------------------------------------------
// File Path: Application/DTO/ApplicationNotificationTypeMap/Validator/UpdateApplicationNotificationTypeMapDtoValidator.cs
using FluentValidation;

namespace Application.DTO.ApplicationNotificationTypeMap.Validator
{
    public class UpdateApplicationNotificationTypeMapDtoValidator : AbstractValidator<UpdateApplicationNotificationTypeMapDto>
    {
        public UpdateApplicationNotificationTypeMapDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.NotificationTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}