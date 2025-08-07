using MediatR;
using Application.DTO.Priority;
using System;

namespace Application.CQRS.Priority.Queries
{
    public class GetPriorityDetailQuery : IRequest<PriorityDto>
    {
        public Guid Id { get; set; }
    }
}