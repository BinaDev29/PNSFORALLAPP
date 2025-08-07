using AutoMapper;
using MediatR;
using Application.CQRS.ClientApplication.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers;

public class DeleteClientApplicationCommandHandler(IGenericRepository<Domain.Models.ClientApplication> repository)
    : IRequestHandler<DeleteClientApplicationCommand, Unit>
{
    public async Task<Unit> Handle(DeleteClientApplicationCommand request, CancellationToken cancellationToken)
    {
        var clientApplication = await repository.Get(request.Id);

        if (clientApplication is null)
        {
            throw new NotFoundException(nameof(Domain.Models.ClientApplication), request.Id);
        }

        await repository.Delete(clientApplication);

        return Unit.Value;
    }
}