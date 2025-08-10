// File Path: Application/CQRS/Priority/Handlers/CreatePriorityCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Commands;
using Application.DTO.Priority.Validator;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers
{
    public class CreatePriorityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreatePriorityCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreatePriorityCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreatePriorityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreatePriorityDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var priority = mapper.Map<Domain.Models.Priority>(request.CreatePriorityDto);
            await unitOfWork.Priorities.Add(priority, cancellationToken);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = priority.Id;
            return response;
        }
    }
}