// File Path: Application/CQRS/Priority/Handlers/DeletePriorityCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Commands;
using Application.Exceptions;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers
{
    public class DeletePriorityCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePriorityCommand, Unit>
    {
        public async Task<Unit> Handle(DeletePriorityCommand request, CancellationToken cancellationToken)
        {
            var priority = await unitOfWork.Priorities.Get(request.Id, cancellationToken);

            if (priority is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Priority), request.Id);
            }

            await unitOfWork.Priorities.Delete(priority, cancellationToken);
            return Unit.Value;
        }
    }
}