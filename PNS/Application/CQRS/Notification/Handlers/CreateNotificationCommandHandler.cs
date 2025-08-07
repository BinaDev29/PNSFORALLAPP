using AutoMapper;
using MediatR;
using Application.CQRS.Notification.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notification.Handlers;

public class CreateNotificationCommandHandler(IGenericRepository<Domain.Models.Notification> repository, IMapper mapper)
    : IRequestHandler<CreateNotificationCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var notification = mapper.Map<Domain.Models.Notification>(request.CreateNotificationDto);

        notification = await repository.Add(notification);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = notification.Id;

        return response;
    }
}