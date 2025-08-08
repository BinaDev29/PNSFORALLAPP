
using ValidationException = Application.Exceptions.ValidationException;
// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/UpdateApplicationNotificationTypeMapCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.ApplicationNotificationTypeMap.Validator;
using FluentValidation;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers;

public class UpdateApplicationNotificationTypeMapCommandHandler(IApplicationNotificationTypeMapRepository repository, IMapper mapper)
    : IRequestHandler<UpdateApplicationNotificationTypeMapCommand, Unit>
{
    public async Task<Unit> Handle(UpdateApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateApplicationNotificationTypeMapDtoValidator();
        var validationResult = await validator.ValidateAsync(request.UpdateApplicationNotificationTypeMapDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var map = await repository.Get(request.UpdateApplicationNotificationTypeMapDto.ClientApplicationId, request.UpdateApplicationNotificationTypeMapDto.NotificationTypeId, cancellationToken);

        // 🟢 ወሳኝ ማስተካከያ: Null ፍተሻ
        if (map is null)
        {
            // መዝገቡ ከሌለ NotFoundException ን ይጥላል
            throw new NotFoundException(nameof(ApplicationNotificationTypeMap), $"{request.UpdateApplicationNotificationTypeMapDto.ClientApplicationId}, {request.UpdateApplicationNotificationTypeMapDto.NotificationTypeId}");
        }

        mapper.Map(request.UpdateApplicationNotificationTypeMapDto, map);
        await repository.Update(map, cancellationToken);

        return Unit.Value;
    }
}