using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Application.DTO.NotificationHistory;

namespace Application.DTO.NotificationHistory.Validator
{
    public class UpdateNotificationHistoryDtoValidator : AbstractValidator<UpdateNotificationHistoryDto>
    {
        public UpdateNotificationHistoryDtoValidator()
        {
            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}