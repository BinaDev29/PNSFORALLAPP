// File Path: Application/CQRS/EmailTemplate/Commands/UpdateEmailTemplateCommand.cs
using Application.DTO.EmailTemplate;
using MediatR;

namespace Application.CQRS.EmailTemplate.Commands
{
    public class UpdateEmailTemplateCommand : IRequest<Unit>
    {
        public required UpdateEmailTemplateDto UpdateEmailTemplateDto { get; set; }
    }
}