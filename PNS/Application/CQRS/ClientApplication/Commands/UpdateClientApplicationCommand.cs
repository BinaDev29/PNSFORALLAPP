using MediatR; // ይህ መስመር መኖሩን አረጋግጥ
using Application.DTO.ClientApplication;

namespace Application.CQRS.ClientApplication.Commands
{
    public class UpdateClientApplicationCommand : IRequest<Unit>
    {
        public required UpdateClientApplicationDto UpdateClientApplicationDto { get; set; }
    }
}