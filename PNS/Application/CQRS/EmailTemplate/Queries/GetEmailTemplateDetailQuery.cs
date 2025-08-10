// File Path: Application/CQRS/EmailTemplate/Queries/GetEmailTemplateDetailQuery.cs
using Application.DTO.EmailTemplate;
using MediatR;
using System;

namespace Application.CQRS.EmailTemplate.Queries
{
    public class GetEmailTemplateDetailQuery : IRequest<EmailTemplateDto>
    {
        public Guid Id { get; set; }
    }
}