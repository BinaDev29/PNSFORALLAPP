using MediatR;
using Application.DTO.ClientApplication;
using System.Collections.Generic;

namespace Application.CQRS.ClientApplication.Queries
{
    public class GetClientApplicationsListQuery : IRequest<IReadOnlyList<ClientApplicationDto>>
    {
    }
}