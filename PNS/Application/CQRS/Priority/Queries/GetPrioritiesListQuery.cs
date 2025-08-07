using MediatR;
using Application.DTO.Priority;
using System.Collections.Generic;

namespace Application.CQRS.Priority.Queries
{
    public class GetPrioritiesListQuery : IRequest<IReadOnlyList<PriorityDto>>
    {
    }
}