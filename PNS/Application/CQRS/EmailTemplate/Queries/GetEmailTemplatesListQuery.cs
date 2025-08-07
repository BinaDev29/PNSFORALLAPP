using MediatR;
using Application.DTO.EmailTemplate;
using System.Collections.Generic;

namespace Application.CQRS.EmailTemplate.Queries
{
    public class GetEmailTemplatesListQuery : IRequest<IReadOnlyList<EmailTemplateDto>>
    {
    }
}