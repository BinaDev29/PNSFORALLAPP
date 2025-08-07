using MediatR;
using System;

namespace Application.CQRS.ClientApplication.Commands
{
    public class DeleteClientApplicationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}