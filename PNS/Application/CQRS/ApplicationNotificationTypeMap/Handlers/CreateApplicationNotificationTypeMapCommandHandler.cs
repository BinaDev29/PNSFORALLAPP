// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/CreateApplicationNotificationTypeMapCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.DTO.ApplicationNotificationTypeMap.Validator;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers
{
    public class CreateApplicationNotificationTypeMapCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateApplicationNotificationTypeMapCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateApplicationNotificationTypeMapDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateApplicationNotificationTypeMapDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var map = mapper.Map<Domain.Models.ApplicationNotificationTypeMap>(request.CreateApplicationNotificationTypeMapDto);
            await unitOfWork.ApplicationNotificationTypeMaps.Add(map, cancellationToken);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = map.Id;
            return response;
        }
    }
}