// File Path: Application/CQRS/ClientApplication/Handlers/GetClientApplicationDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Queries;
using Application.DTO.ClientApplication;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers
{
    public class GetClientApplicationDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientApplicationDetailQuery, ClientApplicationDto>
    {
        public async Task<ClientApplicationDto> Handle(GetClientApplicationDetailQuery request, CancellationToken cancellationToken)
        {
            var clientApplication = await unitOfWork.ClientApplications.Get(request.Id, cancellationToken);

            if (clientApplication == null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.Id);
            }

            return mapper.Map<ClientApplicationDto>(clientApplication);
        }
    }
}