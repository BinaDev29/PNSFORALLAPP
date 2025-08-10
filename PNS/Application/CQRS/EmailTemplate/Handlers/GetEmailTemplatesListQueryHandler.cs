// File Path: Application/CQRS/EmailTemplate/Handlers/GetEmailTemplatesListQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.EmailTemplate.Queries;
using Application.DTO.EmailTemplate;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers
{
    public class GetEmailTemplatesListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetEmailTemplatesListQuery, List<EmailTemplateDto>>
    {
        public async Task<List<EmailTemplateDto>> Handle(GetEmailTemplatesListQuery request, CancellationToken cancellationToken)
        {
            var templates = await unitOfWork.EmailTemplates.GetAll(cancellationToken);
            return mapper.Map<List<EmailTemplateDto>>(templates);
        }
    }
}