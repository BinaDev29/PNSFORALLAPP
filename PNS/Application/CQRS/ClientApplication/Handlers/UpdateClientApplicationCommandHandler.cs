// File Path: Application/CQRS/ClientApplication/Handlers/UpdateClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Commands;
using Application.DTO.ClientApplication.Validator;
using Application.Exceptions;
using AutoMapper;
using MediatR;
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

            // Only update fields that are provided
            if (!string.IsNullOrEmpty(request.UpdateClientApplicationDto.Name))
                clientApplication.Name = request.UpdateClientApplicationDto.Name;
            
            if (request.UpdateClientApplicationDto.Slogan != null)
                clientApplication.Slogan = request.UpdateClientApplicationDto.Slogan;
            
            if (request.UpdateClientApplicationDto.Logo != null)
                clientApplication.Logo = request.UpdateClientApplicationDto.Logo;
            
            if (!string.IsNullOrEmpty(request.UpdateClientApplicationDto.SenderEmail))
                clientApplication.SenderEmail = request.UpdateClientApplicationDto.SenderEmail;
            
            if (!string.IsNullOrEmpty(request.UpdateClientApplicationDto.AppPassword))
                clientApplication.AppPassword = request.UpdateClientApplicationDto.AppPassword;
            
            if (request.UpdateClientApplicationDto.SmsSenderName != null)
                clientApplication.SmsSenderName = request.UpdateClientApplicationDto.SmsSenderName;
            
            if (request.UpdateClientApplicationDto.SmsSenderNumber != null)
                clientApplication.SmsSenderNumber = request.UpdateClientApplicationDto.SmsSenderNumber;

            if (request.UpdateClientApplicationDto.WebhookUrl != null)
                clientApplication.WebhookUrl = request.UpdateClientApplicationDto.WebhookUrl;

            if (request.UpdateClientApplicationDto.WebhookSecret != null)
                clientApplication.WebhookSecret = request.UpdateClientApplicationDto.WebhookSecret;

            await unitOfWork.ClientApplications.Update(clientApplication, cancellationToken);

            // የጠፋው የ Save ጥሪ እዚህ ላይ ታክሏል!
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}