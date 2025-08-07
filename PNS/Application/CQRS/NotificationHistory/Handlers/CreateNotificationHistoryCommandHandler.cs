using AutoMapper;
using MediatR;
using Application.CQRS.NotificationHistory.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.NotificationHistory.Handlers;

// Primary constructor ጥቅም ላይ ውሏል
public class CreateNotificationHistoryCommandHandler(IGenericRepository<Domain.Models.NotificationHistory> repository, IMapper mapper)
    : IRequestHandler<CreateNotificationHistoryCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateNotificationHistoryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var notificationHistory = mapper.Map<Domain.Models.NotificationHistory>(request.CreateNotificationHistoryDto);

        // SentDate ንብረቱ ሞዴሉ ላይ መኖሩን አረጋግጥ
        notificationHistory.SentDate = DateTime.Now;

        notificationHistory = await repository.Add(notificationHistory);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = notificationHistory.Id;

        return response;
    }
}