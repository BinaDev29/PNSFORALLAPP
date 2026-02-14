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
                .Must(r => IsValidEmailOrPhone(r)).WithMessage("Each {PropertyName} must be a valid email address or phone number.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
        
        private bool IsValidEmailOrPhone(string recipient)
        {
            if (string.IsNullOrEmpty(recipient)) return false;
            
            // Basic Email Regex
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            // Basic Phone Regex (allows +, digits, spaces, dashes)
            var phoneRegex = new System.Text.RegularExpressions.Regex(@"^[\+]?[0-9\s\-]{7,15}$");
            
            return emailRegex.IsMatch(recipient) || phoneRegex.IsMatch(recipient);
        }
    }
}