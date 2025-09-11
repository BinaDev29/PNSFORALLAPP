// File Path: Application/CQRS/Sms/Commands/SendSmsCommand.cs
using Application.DTO.Sms;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Sms.Commands
{
    public class SendSmsCommand : IRequest<BaseCommandResponse>
    {
        public required SendSmsNotificationDto SendSmsDto { get; set; }
    }
}