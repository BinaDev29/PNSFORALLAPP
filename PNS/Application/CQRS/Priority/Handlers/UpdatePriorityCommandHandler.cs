using AutoMapper;
using MediatR;
using Application.CQRS.Priority.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers;

// Primary constructor ጥቅም ላይ ውሏል
public class UpdatePriorityCommandHandler(IGenericRepository<Domain.Models.Priority> repository, IMapper mapper)
    : IRequestHandler<UpdatePriorityCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePriorityCommand request, CancellationToken cancellationToken)
    {
        var priority = await repository.Get(request.UpdatePriorityDto.Id);

        // Null check ተስተካክሏል
        if (priority is null)
        {
            throw new NotFoundException(nameof(Domain.Models.Priority), request.UpdatePriorityDto.Id);
        }

        mapper.Map(request.UpdatePriorityDto, priority);

        await repository.Update(priority);

        return Unit.Value;
    }
}