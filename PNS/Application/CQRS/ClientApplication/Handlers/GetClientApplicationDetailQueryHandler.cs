using AutoMapper;
using MediatR;
using Application.CQRS.ClientApplication.Queries;
using Application.Contracts.IRepository;
using Application.DTO.ClientApplication;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers;

public class GetClientApplicationDetailQueryHandler(IGenericRepository<Domain.Models.ClientApplication> repository, IMapper mapper)
    : IRequestHandler<GetClientApplicationDetailQuery, ClientApplicationDto>
{
    public async Task<ClientApplicationDto> Handle(GetClientApplicationDetailQuery request, CancellationToken cancellationToken)
    {
        var clientApplication = await repository.Get(request.Id);

        // የNull check ማስጠንቀቂያን ለማስተካከል
        if (clientApplication is null)
        {
            throw new NotFoundException(nameof(Domain.Models.ClientApplication), request.Id);
        }

        return mapper.Map<ClientApplicationDto>(clientApplication);
    }
}