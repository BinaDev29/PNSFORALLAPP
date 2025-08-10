// File Path: Application/CQRS/EmailTemplate/Handlers/GetEmailTemplateDetailQueryHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.EmailTemplate.Queries;
using Application.DTO.EmailTemplate;
using Application.Exceptions;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers
{
    public class GetEmailTemplateDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetEmailTemplateDetailQuery, EmailTemplateDto>
    {
        public async Task<EmailTemplateDto> Handle(GetEmailTemplateDetailQuery request, CancellationToken cancellationToken)
        {
            var template = await unitOfWork.EmailTemplates.Get(request.Id, cancellationToken);

            if (template is null)
            {
                throw new NotFoundException(nameof(Domain.Models.EmailTemplate), request.Id);
            }

            return mapper.Map<EmailTemplateDto>(template);
        }
    }
}