// File Path: Application/CQRS/Priority/Handlers/GetPrioritiesListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Queries;
using Application.DTO.Priority;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers
{
    public class GetPrioritiesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPrioritiesListQuery, List<PriorityDto>>
    {
        public async Task<List<PriorityDto>> Handle(GetPrioritiesListQuery request, CancellationToken cancellationToken)
        {
            var priorities = await unitOfWork.Priorities.GetAll(cancellationToken);
            return mapper.Map<List<PriorityDto>>(priorities);
        }
    }
}