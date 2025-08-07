// CreateApplicationNotificationTypeMapCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers;

public class CreateApplicationNotificationTypeMapCommandHandler(IApplicationNotificationTypeMapRepository repository, IMapper mapper)
    : IRequestHandler<CreateApplicationNotificationTypeMapCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var map = mapper.Map<Domain.Models.ApplicationNotificationTypeMap>(request.MapDto);

        map = await repository.Add(map);

        response.Success = true;
        response.Message = "Creation Successful.";
        // Id የሚለው ንብረት ስለሌለ በClientApplicationId እንተካዋለን
        response.Id = map.ClientApplicationId;

        return response;
    }
}