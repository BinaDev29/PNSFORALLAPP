// File Path: Application/CQRS/Priority/Commands/CreatePriorityCommand.cs
using Application.DTO.Priority;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Priority.Commands
{
    public class CreatePriorityCommand : IRequest<BaseCommandResponse>
    {
        public required CreatePriorityDto CreatePriorityDto { get; set; }
    }
}