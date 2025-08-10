// File Path: Application/CQRS/ClientApplication/Commands/CreateClientApplicationCommand.cs
using Application.DTO.ClientApplication;
using Application.Responses;
using MediatR;

namespace Application.CQRS.ClientApplication.Commands
{
    public class CreateClientApplicationCommand : IRequest<BaseCommandResponse>
    {
        public required CreateClientApplicationDto CreateClientApplicationDto { get; set; }
    }
}