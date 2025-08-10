// File Path: Application/DTO/ClientApplication/Validator/CreateClientApplicationDtoValidator.cs
using FluentValidation;

namespace Application.DTO.ClientApplication.Validator
{
    public class CreateClientApplicationDtoValidator : AbstractValidator<CreateClientApplicationDto>
    {
        public CreateClientApplicationDtoValidator()
        {
            RuleFor(p => p.AppId).NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}