// File Path: Application/CQRS/ClientApplication/Commands/CreateClientApplicationCommand.cs
using Application.DTO.ClientApplication;
using Application.Responses;
using MediatR;

namespace Application.CQRS.ClientApplication.Commands
{
    // ⭐ ከ class ይልቅ recordን ተጠቀም ⭐
    // ⭐ የ constructor አጻጻፍ በጣም ቀላል ነው። ⭐
    public record CreateClientApplicationCommand(CreateClientApplicationDto CreateClientApplicationDto) : IRequest<BaseCommandResponse>;
}