// GetEmailTemplateDetailQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.EmailTemplate.Queries;
using Application.Contracts.IRepository;
using Application.DTO.EmailTemplate;
using Application.Exceptions;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers;

public class GetEmailTemplateDetailQueryHandler(IGenericRepository<Domain.Models.EmailTemplate> repository, IMapper mapper)
    : IRequestHandler<GetEmailTemplateDetailQuery, EmailTemplateDto>
{
    public async Task<EmailTemplateDto> Handle(GetEmailTemplateDetailQuery request, CancellationToken cancellationToken)
    {
        var emailTemplate = await repository.Get(request.Id);

        if (emailTemplate is null)
        {
            throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.Id);
        }

        return mapper.Map<EmailTemplateDto>(emailTemplate);
    }
}