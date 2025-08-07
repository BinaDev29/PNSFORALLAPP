// GetPriorityDetailQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.Priority.Queries;
using Application.Contracts.IRepository;
using Application.DTO.Priority;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers;

public class GetPriorityDetailQueryHandler(IGenericRepository<Domain.Models.Priority> repository, IMapper mapper)
    : IRequestHandler<GetPriorityDetailQuery, PriorityDto>
{
    public async Task<PriorityDto> Handle(GetPriorityDetailQuery request, CancellationToken cancellationToken)
    {
        var priority = await repository.Get(request.Id);

        if (priority is null)
        {
            throw new NotFoundException(nameof(Domain.Models.Priority), request.Id);
        }

        return mapper.Map<PriorityDto>(priority);
    }
}