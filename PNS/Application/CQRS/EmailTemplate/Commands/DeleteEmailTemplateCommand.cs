using MediatR;
using System;

namespace Application.CQRS.EmailTemplate.Commands
{
    public class DeleteEmailTemplateCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}