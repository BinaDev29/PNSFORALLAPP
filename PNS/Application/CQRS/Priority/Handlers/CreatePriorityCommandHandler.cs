// CreatePriorityCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.Priority.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Priority.Handlers;

public class CreatePriorityCommandHandler(IGenericRepository<Domain.Models.Priority> repository, IMapper mapper)
    : IRequestHandler<CreatePriorityCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreatePriorityCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var priority = mapper.Map<Domain.Models.Priority>(request.CreatePriorityDto);

        priority = await repository.Add(priority);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = priority.Id;

        return response;
    }
}