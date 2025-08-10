// File Path: Application/CQRS/ClientApplication/Queries/GetClientApplicationDetailQuery.cs
using Application.DTO.ClientApplication;
using MediatR;
using System;

namespace Application.CQRS.ClientApplication.Queries
{
    public class GetClientApplicationDetailQuery : IRequest<ClientApplicationDto>
    {
        public Guid Id { get; set; }
    }
}