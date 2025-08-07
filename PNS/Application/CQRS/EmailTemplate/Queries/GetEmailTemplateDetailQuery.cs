using MediatR;
using Application.DTO.EmailTemplate;
using System;

namespace Application.CQRS.EmailTemplate.Queries
{
    public class GetEmailTemplateDetailQuery : IRequest<EmailTemplateDto>
    {
        public Guid Id { get; set; }
    }
}