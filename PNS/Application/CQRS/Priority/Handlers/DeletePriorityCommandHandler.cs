// DeletePriorityCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.Priority.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers;

public class DeletePriorityCommandHandler(IGenericRepository<Domain.Models.Priority> repository)
    : IRequestHandler<DeletePriorityCommand, Unit>
{
    public async Task<Unit> Handle(DeletePriorityCommand request, CancellationToken cancellationToken)
    {
        var priority = await repository.Get(request.Id);

        if (priority is null)
        {
            throw new NotFoundException(nameof(Domain.Models.Priority), request.Id);
        }

        await repository.Delete(priority);

        return Unit.Value;
    }
}