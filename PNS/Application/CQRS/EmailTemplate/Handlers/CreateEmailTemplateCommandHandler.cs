// File Path: Application/CQRS/EmailTemplate/Handlers/CreateEmailTemplateCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.EmailTemplate.Commands;
using Application.DTO.EmailTemplate.Validator;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailTemplate.Handlers
{
    public class CreateEmailTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateEmailTemplateCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateEmailTemplateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateEmailTemplateDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var emailTemplate = mapper.Map<Domain.Models.EmailTemplate>(request.CreateEmailTemplateDto);
            await unitOfWork.EmailTemplates.Add(emailTemplate, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = emailTemplate.Id;
            return response;
        }
    }
}