// File Path: Application/Profiles/MappingProfile.cs
using System;
using System.Linq;
using Application.DTO.ApplicationNotificationTypeMap;
using Application.DTO.ClientApplication;
using Application.DTO.EmailTemplate;
using Application.DTO.Notification;
using Application.DTO.NotificationHistory;
using Application.DTO.NotificationType;
using Application.DTO.Priority;
using AutoMapper;
using Domain.Models;
using Domain.ValueObjects;

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
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To.Select(x => x.ToString() ?? string.Empty).ToList()))
                .ReverseMap();

            CreateMap<CreateNotificationDto, Notification>()
                  .ForMember(dest => dest.To, opt => opt.MapFrom(src => MapToRecipients(src.To)));
            CreateMap<Notification, CreateNotificationDto>()
                  .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To.Select(x => x.ToString() ?? string.Empty).ToList()));

            CreateMap<UpdateNotificationDto, Notification>()
                  .ForMember(dest => dest.To, opt => opt.MapFrom(src => MapToRecipients(src.To)));
            CreateMap<Notification, UpdateNotificationDto>()
                  .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To.Select(x => x.ToString() ?? string.Empty).ToList()));

            // NotificationHistory
            CreateMap<NotificationHistory, NotificationHistoryDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Notification != null ? src.Notification.Title : string.Empty))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Notification != null ? src.Notification.Message : string.Empty))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Notification != null && src.Notification.To != null 
                    ? string.Join(", ", src.Notification.To.Select(x => x.ToString())) 
                    : string.Empty))
                .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.Notification != null && src.Notification.NotificationType != null 
                    ? src.Notification.NotificationType.Name 
                    : string.Empty))
                 .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Notification != null ? src.Notification.ErrorMessage : null))
                .ReverseMap();

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

        private List<object> MapToRecipients(List<string> to)
        {
            if (to == null) return new List<object>();
            return to.Select(t =>
            {
                if (EmailAddress.IsValidEmail(t)) return (object)EmailAddress.Create(t);
                if (PhoneNumber.IsValid(t)) return (object)PhoneNumber.Create(t);
                return (object)t;
            }).ToList();
        }
    }
}