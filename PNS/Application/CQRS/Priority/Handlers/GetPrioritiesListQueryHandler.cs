// GetPrioritiesListQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.Priority.Queries;
using Application.Contracts.IRepository;
using Application.DTO.Priority;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.CQRS.Priority.Handlers;

public class GetPrioritiesListQueryHandler(IGenericRepository<Domain.Models.Priority> repository, IMapper mapper)
    : IRequestHandler<GetPrioritiesListQuery, IReadOnlyList<PriorityDto>>
{
    public async Task<IReadOnlyList<PriorityDto>> Handle(GetPrioritiesListQuery request, CancellationToken cancellationToken)
    {
        var priorities = await repository.GetAll();
        return mapper.Map<IReadOnlyList<PriorityDto>>(priorities);
    }
}