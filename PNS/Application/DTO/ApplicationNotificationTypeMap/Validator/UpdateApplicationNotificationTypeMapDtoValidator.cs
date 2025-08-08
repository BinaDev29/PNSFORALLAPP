using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.ApplicationNotificationTypeMap;

namespace Application.DTO.ApplicationNotificationTypeMap.Validator
{
    public class UpdateApplicationNotificationTypeMapDtoValidator : AbstractValidator<UpdateApplicationNotificationTypeMapDto>
    {
        public UpdateApplicationNotificationTypeMapDtoValidator()
        {
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.NotificationTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}