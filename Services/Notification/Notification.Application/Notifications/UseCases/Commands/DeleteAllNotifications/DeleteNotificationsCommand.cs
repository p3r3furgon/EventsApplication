using MediatR;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteAllNotifications
{
    public record DeleteNotificationsCommand() : IRequest<DeleteNotificationsResponse>;
    public record DeleteNotificationsResponse(string Result);

}
