using MediatR;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications
{
    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, GetAllNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;

        public GetAllNotificationsQueryHandler(INotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        public async Task<GetAllNotificationsResponse> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationsRepository.GetAll();
            return new GetAllNotificationsResponse(notifications);
        }
    }
}
