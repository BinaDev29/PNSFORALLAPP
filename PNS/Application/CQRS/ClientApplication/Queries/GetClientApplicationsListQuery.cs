// File Path: Application/CQRS/ClientApplication/Queries/GetClientApplicationsListQuery.cs
using Application.DTO.ClientApplication;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.ClientApplication.Queries
{
    public class GetClientApplicationsListQuery : IRequest<List<ClientApplicationDto>>
    {
    }
}