// File Path: Application/CQRS/Priority/Handlers/UpdatePriorityCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.Priority.Commands;
using Application.DTO.Priority.Validator;
using Application.Exceptions; // This is the correct namespace for your custom exception
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

// NOTE: The 'using System.ComponentModel.DataAnnotations;' has been removed to resolve the ambiguity.

namespace Application.CQRS.Priority.Handlers
{
    public class UpdatePriorityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UpdatePriorityDtoValidator validator) : IRequestHandler<UpdatePriorityCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePriorityCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request.UpdatePriorityDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                // The ValidationException from Application.Exceptions is now unambiguously used.
                throw new ValidationException(validationResult);
            }

            var priority = await unitOfWork.Priorities.Get(request.UpdatePriorityDto.Id, cancellationToken);

            if (priority is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Priority), request.UpdatePriorityDto.Id);
            }

            mapper.Map(request.UpdatePriorityDto, priority);
            await unitOfWork.Priorities.Update(priority, cancellationToken);

            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}