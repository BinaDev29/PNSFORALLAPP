using FluentValidation;
using Application.DTO.NotificationType;

namespace Application.DTO.NotificationType.Validator
{
    public class UpdateNotificationTypeDtoValidator : AbstractValidator<UpdateNotificationTypeDto>
    {
        public UpdateNotificationTypeDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.Subject)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}