using AutoMapper;
using MediatR;
using Application.CQRS.NotificationType.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationType.Handlers;

public class CreateNotificationTypeCommandHandler(IGenericRepository<Domain.Models.NotificationType> repository, IMapper mapper)
    : IRequestHandler<CreateNotificationTypeCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateNotificationTypeCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var notificationType = mapper.Map<Domain.Models.NotificationType>(request.CreateNotificationTypeDto);

        notificationType = await repository.Add(notificationType);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = notificationType.Id;

        return response;
    }
}