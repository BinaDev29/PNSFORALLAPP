using AutoMapper;
using MediatR;
using Application.CQRS.ClientApplication.Commands;
using Application.Contracts.IRepository; // ትክክለኛው የ using directive
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers;

// IClientApplicationRepositoryን በ IGenericRepository<Domain.Models.ClientApplication> እንተካለን
public class CreateClientApplicationCommandHandler(IGenericRepository<Domain.Models.ClientApplication> clientApplicationRepository, IMapper mapper)
    : IRequestHandler<CreateClientApplicationCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateClientApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var clientApplication = mapper.Map<Domain.Models.ClientApplication>(request.CreateClientApplicationDto);

        clientApplication = await clientApplicationRepository.Add(clientApplication);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = clientApplication.Id;

        return response;
    }
}