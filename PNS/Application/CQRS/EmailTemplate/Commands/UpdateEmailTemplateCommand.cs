using MediatR;
using Application.DTO.EmailTemplate;

namespace Application.CQRS.EmailTemplate.Commands
{
    public class UpdateEmailTemplateCommand : IRequest<Unit>
    {
        public required UpdateEmailTemplateDto UpdateEmailTemplateDto { get; set; }
    }
}