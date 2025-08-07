using AutoMapper;
using Domain.Models;
using Application.DTO.ClientApplication;
using Application.DTO.Notification;
using Application.DTO.NotificationHistory;
using Application.DTO.NotificationType;
using Application.DTO.Priority;
using Application.DTO.EmailTemplate;
using Application.DTO.ApplicationNotificationTypeMap; // አዲሱ የጨመርነው

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ClientApplication
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap();
            CreateMap<CreateClientApplicationDto, ClientApplication>();
            CreateMap<UpdateClientApplicationDto, ClientApplication>();

            // Notification
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<UpdateNotificationDto, Notification>();

            // NotificationHistory
            CreateMap<NotificationHistory, NotificationHistoryDto>().ReverseMap();
            CreateMap<CreateNotificationHistoryDto, NotificationHistory>();
            CreateMap<UpdateNotificationHistoryDto, NotificationHistory>();

            // NotificationType
            CreateMap<NotificationType, NotificationTypeDto>().ReverseMap();
            CreateMap<CreateNotificationTypeDto, NotificationType>();
            CreateMap<UpdateNotificationTypeDto, NotificationType>();

            // Priority
            CreateMap<Priority, PriorityDto>().ReverseMap();
            CreateMap<CreatePriorityDto, Priority>();
            CreateMap<UpdatePriorityDto, Priority>();

            // EmailTemplate
            CreateMap<EmailTemplate, EmailTemplateDto>().ReverseMap();
            CreateMap<CreateEmailTemplateDto, EmailTemplate>();
            CreateMap<UpdateEmailTemplateDto, EmailTemplate>();

            // ApplicationNotificationTypeMap (አዲሱ የጨመርነው)
            CreateMap<ApplicationNotificationTypeMap, ApplicationNotificationTypeMapDto>().ReverseMap();
            CreateMap<CreateApplicationNotificationTypeMapDto, ApplicationNotificationTypeMap>();
            CreateMap<UpdateApplicationNotificationTypeMapDto, ApplicationNotificationTypeMap>();
        }
    }
}