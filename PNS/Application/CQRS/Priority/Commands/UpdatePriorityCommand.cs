// File Path: Application/CQRS/Priority/Commands/UpdatePriorityCommand.cs
using Application.DTO.Priority;
using MediatR;

namespace Application.CQRS.Priority.Commands
{
    public class UpdatePriorityCommand : IRequest<Unit>
    {
        public required UpdatePriorityDto UpdatePriorityDto { get; set; }
    }
}