// File Path: Application/CQRS/ClientApplication/Commands/UpdateClientApplicationCommand.cs
using Application.DTO.ClientApplication;
using MediatR;

namespace Application.CQRS.ClientApplication.Commands
{
    public class UpdateClientApplicationCommand : IRequest<Unit>
    {
        public required UpdateClientApplicationDto UpdateClientApplicationDto { get; set; }
    }
}