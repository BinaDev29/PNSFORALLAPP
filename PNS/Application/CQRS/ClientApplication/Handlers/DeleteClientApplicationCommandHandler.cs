// File Path: Application/CQRS/ClientApplication/Handlers/DeleteClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Commands;
using Application.Exceptions;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers
{
    public class DeleteClientApplicationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteClientApplicationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var clientApplication = await unitOfWork.ClientApplications.Get(request.Id, cancellationToken);

            if (clientApplication == null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.Id);
            }

            await unitOfWork.ClientApplications.Delete(clientApplication, cancellationToken);
            return Unit.Value;
        }
    }
}