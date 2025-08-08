// File Path: Application/CQRS/ApplicationNotificationTypeMap/Handlers/CreateApplicationNotificationTypeMapCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using Application.DTO.ApplicationNotificationTypeMap.Validator;
using System.Linq;
using FluentValidation; // FluentValidationን ማስገባት አይዘንጋ

namespace Application.CQRS.ApplicationNotificationTypeMap.Handlers;

public class CreateApplicationNotificationTypeMapCommandHandler(IApplicationNotificationTypeMapRepository repository, IMapper mapper)
    : IRequestHandler<CreateApplicationNotificationTypeMapCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateApplicationNotificationTypeMapCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var validator = new CreateApplicationNotificationTypeMapDtoValidator();
        var validationResult = await validator.ValidateAsync(request.MapDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            response.Success = false;
            response.Message = "Creation Failed";
            response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            return response;
        }

        var map = mapper.Map<Domain.Models.ApplicationNotificationTypeMap>(request.MapDto);
        await repository.Add(map, cancellationToken);

        response.Success = true;
        response.Message = "Creation Successful.";

        return response;
    }
}