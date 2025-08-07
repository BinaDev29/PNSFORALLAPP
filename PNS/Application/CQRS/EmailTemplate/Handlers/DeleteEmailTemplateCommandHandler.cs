// DeleteEmailTemplateCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.EmailTemplate.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers;

public class DeleteEmailTemplateCommandHandler(IGenericRepository<Domain.Models.EmailTemplate> repository)
    : IRequestHandler<DeleteEmailTemplateCommand, Unit>
{
    public async Task<Unit> Handle(DeleteEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        var emailTemplate = await repository.Get(request.Id);

        if (emailTemplate is null)
        {
            throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.Id);
        }

        await repository.Delete(emailTemplate);

        return Unit.Value;
    }
}