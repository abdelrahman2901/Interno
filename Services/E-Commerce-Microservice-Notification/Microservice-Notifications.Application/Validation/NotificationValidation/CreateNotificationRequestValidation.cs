using FluentValidation;
using Microservice_Notifications.Application.Features.Notification.Command.CreateNotifcationCmd;

namespace Microservice_Notifications.Application.Validation.NotificationValidation
{
    public class CreateNotificationRequestValidation : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidation()
        {
            RuleFor(r => r.Message).NotEmpty().WithMessage("Message Cant Be Empty");
            RuleFor(r => r.UserID).NotEmpty().WithMessage("UserID Cant Be Empty");
        }
    }
}
