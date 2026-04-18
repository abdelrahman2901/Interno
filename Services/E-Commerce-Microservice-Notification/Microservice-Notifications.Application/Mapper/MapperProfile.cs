using AutoMapper;
using Microservice_Notifications.Entity;
using Microservice_Notification.Core.DTO.NotificationDTO;
using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;

namespace Microservice_Notifications.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Notifications, NotificationResponse>();
            CreateMap<CreateNotificationRequest, Notifications>()
                .ForMember(dest=>dest.NotificationID,opt=>Guid.NewGuid())
                .ForMember(dest=>dest.CreatedAt,opt=>opt.MapFrom(src=>DateTime.UtcNow));
        }
    }
}
