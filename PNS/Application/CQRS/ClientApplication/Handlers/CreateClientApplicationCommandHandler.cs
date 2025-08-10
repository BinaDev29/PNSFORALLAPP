// File Path: Application/CQRS/ClientApplication/Handlers/CreateClientApplicationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.CQRS.ClientApplication.Commands;
using Application.DTO.ClientApplication;
using Application.DTO.ClientApplication.Validator;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;
using System; // Guidን ለመጠቀም ይህንን library ጨምር
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplication.Handlers
{
    public class CreateClientApplicationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateClientApplicationCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateClientApplicationDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CreateClientApplicationDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var clientApplication = mapper.Map<Domain.Models.ClientApplication>(request.CreateClientApplicationDto);

            // እዚህ ላይ ለ Key property ዋጋ መመደብ አለብህ
            clientApplication.Key = Guid.NewGuid().ToString();

            await unitOfWork.ClientApplications.Add(clientApplication, cancellationToken);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Id = clientApplication.Id;
            return response;
        }
    }
}