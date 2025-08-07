// UpdateEmailTemplateCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.EmailTemplate.Commands;
using Application.Contracts.IRepository;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers;

// Primary constructor ጥቅም ላይ ውሏል
public class UpdateEmailTemplateCommandHandler(IGenericRepository<Domain.Models.EmailTemplate> repository, IMapper mapper)
    : IRequestHandler<UpdateEmailTemplateCommand, Unit>
{
    public async Task<Unit> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        var emailTemplate = await repository.Get(request.UpdateEmailTemplateDto.Id);

        // Null check ተስተካክሏል
        if (emailTemplate is null)
        {
            throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.UpdateEmailTemplateDto.Id);
        }

        mapper.Map(request.UpdateEmailTemplateDto, emailTemplate);

        await repository.Update(emailTemplate);

        return Unit.Value;
    }
}