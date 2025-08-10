// File Path: Application/CQRS/Priority/Queries/GetPrioritiesListQuery.cs
using Application.DTO.Priority;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Priority.Queries
{
    public class GetPrioritiesListQuery : IRequest<List<PriorityDto>>
    {
    }
}