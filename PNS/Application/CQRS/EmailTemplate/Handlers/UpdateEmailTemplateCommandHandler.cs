// File Path: Application/CQRS/EmailTemplate/Handlers/UpdateEmailTemplateCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.EmailTemplate.Commands;
using Application.DTO.EmailTemplate.Validator;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers
{
    // Inject the validator for better testability and design
    public class UpdateEmailTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UpdateEmailTemplateDtoValidator validator) : IRequestHandler<UpdateEmailTemplateCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            // Use the injected validator instance
            var validationResult = await validator.ValidateAsync(request.UpdateEmailTemplateDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            var emailTemplate = await unitOfWork.EmailTemplates.Get(request.UpdateEmailTemplateDto.Id, cancellationToken);

            if (emailTemplate is null)
            {
                throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.UpdateEmailTemplateDto.Id);
            }

            mapper.Map(request.UpdateEmailTemplateDto, emailTemplate);
            await unitOfWork.EmailTemplates.Update(emailTemplate, cancellationToken);

            // Critical fix: Add the save call to persist the update
            await unitOfWork.Save(cancellationToken);

            return Unit.Value;
        }
    }
}