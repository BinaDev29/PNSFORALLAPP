// GetEmailTemplatesListQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.EmailTemplate.Queries;
using Application.Contracts.IRepository;
using Application.DTO.EmailTemplate;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.CQRS.EmailTemplate.Handlers;

public class GetEmailTemplatesListQueryHandler(IGenericRepository<Domain.Models.EmailTemplate> repository, IMapper mapper)
    : IRequestHandler<GetEmailTemplatesListQuery, IReadOnlyList<EmailTemplateDto>>
{
    public async Task<IReadOnlyList<EmailTemplateDto>> Handle(GetEmailTemplatesListQuery request, CancellationToken cancellationToken)
    {
        var emailTemplates = await repository.GetAll();
        return mapper.Map<IReadOnlyList<EmailTemplateDto>>(emailTemplates);
    }
}