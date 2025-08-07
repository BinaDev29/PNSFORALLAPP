// CreateEmailTemplateCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.CQRS.EmailTemplate.Commands;
using Application.Contracts.IRepository;
using Application.Responses;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers;

public class CreateEmailTemplateCommandHandler(IGenericRepository<Domain.Models.EmailTemplate> repository, IMapper mapper)
    : IRequestHandler<CreateEmailTemplateCommand, BaseCommandResponse>
{
    public async Task<BaseCommandResponse> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseCommandResponse();
        var emailTemplate = mapper.Map<Domain.Models.EmailTemplate>(request.CreateEmailTemplateDto);

        emailTemplate = await repository.Add(emailTemplate);

        response.Success = true;
        response.Message = "Creation Successful.";
        response.Id = emailTemplate.Id;

        return response;
    }
}