using AutoMapper;
using MediatR;
using Application.CQRS.ClientApplication.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers;

public class UpdateClientApplicationCommandHandler(IGenericRepository<Domain.Models.ClientApplication> repository, IMapper mapper)
    : IRequestHandler<UpdateClientApplicationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateClientApplicationCommand request, CancellationToken cancellationToken)
    {
        var clientApplication = await repository.Get(request.UpdateClientApplicationDto.Id);

        if (clientApplication is null) // Null check ተስተካክሏል
        {
            throw new NotFoundException(nameof(Domain.Models.ClientApplication), request.UpdateClientApplicationDto.Id);
        }

        mapper.Map(request.UpdateClientApplicationDto, clientApplication);

        await repository.Update(clientApplication);

        return Unit.Value;
    }
}