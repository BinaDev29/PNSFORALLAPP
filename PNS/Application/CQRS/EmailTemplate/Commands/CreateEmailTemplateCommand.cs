using MediatR;
using Application.DTO.EmailTemplate;
using Application.Responses;

namespace Application.CQRS.EmailTemplate.Commands
{
    public class CreateEmailTemplateCommand : IRequest<BaseCommandResponse>
    {
        public required CreateEmailTemplateDto CreateEmailTemplateDto { get; set; }
    }
}