// File Path: Application/CQRS/Priority/Commands/DeletePriorityCommand.cs
using MediatR;
using System;

namespace Application.CQRS.Priority.Commands
{
    public class DeletePriorityCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}