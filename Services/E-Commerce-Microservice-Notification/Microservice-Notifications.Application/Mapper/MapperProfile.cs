using AutoMapper;
using Microservice_Notifications.Entity;
using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;
using Microservice_Notifications.Domain.DTO.NotificationDTO;

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
