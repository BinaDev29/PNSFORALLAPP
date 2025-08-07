// ApplicationNotificationTypeMapDto.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ApplicationNotificationTypeMap
{
    public class ApplicationNotificationTypeMapDto
    {
        public Guid ClientApplicationId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public Guid? EmailTemplateId { get; set; }
    }
}