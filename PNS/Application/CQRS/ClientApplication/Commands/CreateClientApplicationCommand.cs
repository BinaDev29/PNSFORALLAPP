// File Path: Application/CQRS/ClientApplication/Commands/CreateClientApplicationCommand.cs
using Application.DTO.ClientApplication;
using Application.Responses;
using MediatR;

namespace Application.CQRS.ClientApplication.Commands
{
   
    public record CreateClientApplicationCommand(CreateClientApplicationDto CreateClientApplicationDto) : IRequest<BaseCommandResponse>;
}