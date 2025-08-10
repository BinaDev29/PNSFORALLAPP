// File Path: Application/CQRS/EmailTemplate/Queries/GetEmailTemplatesListQuery.cs
using Application.DTO.EmailTemplate;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.EmailTemplate.Queries
{
    public class GetEmailTemplatesListQuery : IRequest<List<EmailTemplateDto>>
    {
    }
}