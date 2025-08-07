using MediatR;
using Application.DTO.ClientApplication;
using Application.Responses;

namespace Application.CQRS.ClientApplication.Commands
{
    public class CreateClientApplicationCommand : IRequest<BaseCommandResponse>
    {
        public required CreateClientApplicationDto CreateClientApplicationDto { get; set; }
    }
}