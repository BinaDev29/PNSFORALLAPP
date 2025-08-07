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
            // Update ሲደረግ የሚያስፈልገውን validation እዚህ ጋር ትጽፋለህ።
            // ለምሳሌ IsEnabled ባዶ እንዳይሆን ማድረግ ከፈለግህ።
            // RuleFor(p => p.IsEnabled).NotNull().WithMessage("{PropertyName} is required.");
        }
    }
}