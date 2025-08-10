// File Path: Application/Profiles/MappingProfile.cs
using Application.DTO.ApplicationNotificationTypeMap;
using Application.DTO.ClientApplication;
using Application.DTO.EmailTemplate;
using Application.DTO.Notification;
using Application.DTO.NotificationHistory;
using Application.DTO.NotificationType;
using Application.DTO.Priority;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ClientApplication
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, CreateClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, UpdateClientApplicationDto>().ReverseMap();

            // Notification
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();

            // NotificationHistory
            CreateMap<NotificationHistory, NotificationHistoryDto>().ReverseMap();
            CreateMap<NotificationHistory, CreateNotificationHistoryDto>().ReverseMap();

            // NotificationType
            CreateMap<NotificationType, NotificationTypeDto>().ReverseMap();
            CreateMap<NotificationType, CreateNotificationTypeDto>().ReverseMap();
            CreateMap<NotificationType, UpdateNotificationTypeDto>().ReverseMap();

            // ApplicationNotificationTypeMap
            CreateMap<ApplicationNotificationTypeMap, ApplicationNotificationTypeMapDto>().ReverseMap();
            CreateMap<ApplicationNotificationTypeMap, CreateApplicationNotificationTypeMapDto>().ReverseMap();
            CreateMap<ApplicationNotificationTypeMap, UpdateApplicationNotificationTypeMapDto>().ReverseMap();

            // EmailTemplate
            CreateMap<EmailTemplate, EmailTemplateDto>().ReverseMap();
            CreateMap<EmailTemplate, CreateEmailTemplateDto>().ReverseMap();
            CreateMap<EmailTemplate, UpdateEmailTemplateDto>().ReverseMap();

            // Priority
            CreateMap<Priority, PriorityDto>().ReverseMap();
            CreateMap<Priority, CreatePriorityDto>().ReverseMap();
            CreateMap<Priority, UpdatePriorityDto>().ReverseMap();
        }
    }
}