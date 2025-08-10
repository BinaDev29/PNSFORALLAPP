// File Path: Application/CQRS/Priority/Handlers/UpdatePriorityCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Commands;
using Application.DTO.Priority.Validator;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers
{
    public class UpdatePriorityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdatePriorityCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePriorityCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdatePriorityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdatePriorityDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var priority = await unitOfWork.Priorities.Get(request.UpdatePriorityDto.Id, cancellationToken);

            if (priority is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Priority), request.UpdatePriorityDto.Id);
            }

            mapper.Map(request.UpdatePriorityDto, priority);
            await unitOfWork.Priorities.Update(priority, cancellationToken);

            return Unit.Value;
        }
    }
}