// File Path: Application/CQRS/Priority/Queries/GetPriorityDetailQuery.cs
using Application.DTO.Priority;
using MediatR;
using System;

namespace Application.CQRS.Priority.Queries
{
    public class GetPriorityDetailQuery : IRequest<PriorityDto>
    {
        public Guid Id { get; set; }
    }
}