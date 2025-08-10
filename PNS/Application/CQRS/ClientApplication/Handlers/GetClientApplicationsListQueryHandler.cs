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
    public class GetClientApplicationsListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientApplicationsListQuery, List<ClientApplicationDto>>
    {
        public async Task<List<ClientApplicationDto>> Handle(GetClientApplicationsListQuery request, CancellationToken cancellationToken)
        {
            var clientApplications = await unitOfWork.ClientApplications.GetAll(cancellationToken);
            return mapper.Map<List<ClientApplicationDto>>(clientApplications);
        }
    }
}