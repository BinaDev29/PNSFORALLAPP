// File Path: Application/CQRS/Priority/Handlers/GetPriorityDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Queries;
using Application.DTO.Priority;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers
{
    public class GetPriorityDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPriorityDetailQuery, PriorityDto>
    {
        public async Task<PriorityDto> Handle(GetPriorityDetailQuery request, CancellationToken cancellationToken)
        {
            var priority = await unitOfWork.Priorities.Get(request.Id, cancellationToken);

            if (priority is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Priority), request.Id);
            }

            return mapper.Map<PriorityDto>(priority);
        }
    }
}