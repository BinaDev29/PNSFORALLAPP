// File Path: Application/CQRS/EmailTemplate/Commands/CreateEmailTemplateCommand.cs
using Application.DTO.EmailTemplate;
using Application.Responses;
using MediatR;

namespace Application.CQRS.EmailTemplate.Commands
{
    public class CreateEmailTemplateCommand : IRequest<BaseCommandResponse>
    {
        public required CreateEmailTemplateDto CreateEmailTemplateDto { get; set; }
    }
}