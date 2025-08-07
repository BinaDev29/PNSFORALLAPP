using MediatR;
using Application.DTO.Priority;
using Application.Responses;

namespace Application.CQRS.Priority.Commands
{
    public class CreatePriorityCommand : IRequest<BaseCommandResponse>
    {
        public required CreatePriorityDto CreatePriorityDto { get; set; }
    }
}