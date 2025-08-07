using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.Notification;

namespace Application.DTO.Notification.Validator
{
    public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationDtoValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.");
            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}