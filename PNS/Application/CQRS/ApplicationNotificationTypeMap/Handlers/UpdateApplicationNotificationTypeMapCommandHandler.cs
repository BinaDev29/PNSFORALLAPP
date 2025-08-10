// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/UpdateApplicationNotificationTypeMapCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.DTO.ApplicationNotificationTypeMap.Validator;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class UpdateApplicationNotificationTypeMapCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateApplicationNotificationTypeMapCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateApplicationNotificationTypeMapDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateApplicationNotificationTypeMapDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var map = await unitOfWork.ApplicationNotificationTypeMaps.Get(request.UpdateApplicationNotificationTypeMapDto.Id, cancellationToken);

            if (map is null)
            {
                throw new NotFoundException(nameof(Domain.Models.ApplicationNotificationTypeMap), request.UpdateApplicationNotificationTypeMapDto.Id);
            }

            mapper.Map(request.UpdateApplicationNotificationTypeMapDto, map);
            await unitOfWork.ApplicationNotificationTypeMaps.Update(map, cancellationToken);

            return Unit.Value;
        }
    }
}