// File Path: Application/CQRS/ClientApplication/Queries/GetClientApplicationsListQuery.cs
using Application.DTO.ClientApplication;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.ClientApplication.Queries
{
    public class GetClientApplicationsListQuery : IRequest<List<ClientApplicationDto>>
    {
        public string? UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}