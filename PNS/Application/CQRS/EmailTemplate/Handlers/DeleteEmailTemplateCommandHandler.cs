// File Path: Application/CQRS/EmailTemplate/Handlers/DeleteEmailTemplateCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.EmailTemplate.Commands;
using Application.Exceptions;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers
{
    public class DeleteEmailTemplateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEmailTemplateCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var emailTemplate = await unitOfWork.EmailTemplates.Get(request.Id, cancellationToken);

            if (emailTemplate is null)
            {
                throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.Id);
            }

            await unitOfWork.EmailTemplates.Delete(emailTemplate, cancellationToken);
            return Unit.Value;
        }
    }
}