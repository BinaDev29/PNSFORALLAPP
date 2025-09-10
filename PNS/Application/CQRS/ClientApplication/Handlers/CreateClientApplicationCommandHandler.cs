// File Path: Application/CQRS/ClientApplication/Handlers/CreateClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Commands;
using Application.DTO.ClientApplication;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

// የተደጋገመው CreateClientApplicationCommand ትርጉም እዚህ ላይ ተወግዷል
public class CreateClientApplicationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateClientApplicationCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateClientApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();

        // EncryptionService ንበዲፔንደንሲ ኢንጄክሽን ማስገባት ይመከራል
        // var encryptedPassword = encryptionService.Encrypt(request.CreateClientApplicationDto.AppPassword);
        // var clientApplication = mapper.Map<ClientApplication>(request.CreateClientApplicationDto);
        // clientApplication.AppPassword = encryptedPassword;

        // ለጊዜው፣ በቀጥታ እንጠቀማለን
        var clientApplication = mapper.Map<ClientApplication>(request.CreateClientApplicationDto);

        await unitOfWork.ClientApplications.Add(clientApplication, cancellationToken);
        await unitOfWork.Save(cancellationToken);

        response.Success = true;
        response.Message = "Client Application Creation Successful";
        response.Id = clientApplication.Id;

        return response;
    }
}