using MediatR;
using Application.DTO.ClientApplication;
using System;

namespace Application.CQRS.ClientApplication.Queries
{
    public class GetClientApplicationDetailQuery : IRequest<ClientApplicationDto>
    {
        public Guid Id { get; set; }
    }
}