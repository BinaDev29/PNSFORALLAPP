// File Path: Application/CQRS/Notification/Commands/SendPushNotificationCommand.cs
using MediatR;
using Domain.Models; // 🟢 ትክክለኛውን የ`model` namespace መጠቀም
using Domain;

namespace Application.CQRS.Notification.Commands
{
    public class SendPushNotificationCommand : IRequest<Unit>
    {
        public required Domain.Models.Notification Notification { get; set; } // 🟢 `required` modifier added
    }
}