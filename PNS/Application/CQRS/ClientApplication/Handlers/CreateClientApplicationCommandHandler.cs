// File Path: Application/CQRS/ClientApplication/Handlers/CreateClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.DTO.ClientApplication;
using Application.Responses;
using AutoMapper;
using MediatR;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

// CreateClientApplicationCommand and CreateClientApplicationDto are assumed to exist
public record CreateClientApplicationCommand(CreateClientApplicationDto CreateClientApplicationDto) : IRequest<BaseCommandResponse>;

public class CreateClientApplicationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateClientApplicationCommand, BaseCommandResponse>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseCommandResponse> Handle(CreateClientApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();

        var encryptedPassword = EncryptionService.Encrypt(request.CreateClientApplicationDto.AppPassword);

        var clientApplication = _mapper.Map<ClientApplication>(request.CreateClientApplicationDto);
        clientApplication.AppPassword = encryptedPassword; 

        await _unitOfWork.ClientApplications.Add(clientApplication, cancellationToken);
        await _unitOfWork.Save(cancellationToken);

        response.Success = true;
        response.Message = "Client Application Creation Successful";
        response.Id = clientApplication.Id;

        return response;
    }
}