// File Path: Application/CQRS/ClientApplication/Handlers/UpdateClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Commands;
using Application.DTO.ClientApplication;
using Application.DTO.ClientApplication.Validator;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers
{
    public class UpdateClientApplicationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateClientApplicationCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateClientApplicationDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateClientApplicationDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var clientApplication = await unitOfWork.ClientApplications.Get(request.UpdateClientApplicationDto.Id, cancellationToken);

            if (clientApplication is null)
            {
                throw new NotFoundException(nameof(Domain.Models.ClientApplication), request.UpdateClientApplicationDto.Id);
            }

            mapper.Map(request.UpdateClientApplicationDto, clientApplication);
            await unitOfWork.ClientApplications.Update(clientApplication, cancellationToken);

            return Unit.Value;
        }
    }
}