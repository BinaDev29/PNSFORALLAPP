// File Path: Application/CQRS/ClientApplication/Handlers/GetClientApplicationsListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Queries;
using Application.DTO.ClientApplication;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers
{
    public class GetClientApplicationsListQueryHandler(IClientApplicationRepository repository, IMapper mapper)
        : IRequestHandler<GetClientApplicationsListQuery, IReadOnlyList<ClientApplicationDto>>
    {
        public async Task<IReadOnlyList<ClientApplicationDto>> Handle(GetClientApplicationsListQuery request, CancellationToken cancellationToken)
        {
            var clientApplications = await repository.GetAll(cancellationToken);
            return mapper.Map<IReadOnlyList<ClientApplicationDto>>(clientApplications);
        }
    }
}