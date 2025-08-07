using MediatR;
using Application.DTO.Priority;

namespace Application.CQRS.Priority.Commands
{
    public class UpdatePriorityCommand : IRequest<Unit>
    {
        public required UpdatePriorityDto UpdatePriorityDto { get; set; }
    }
}