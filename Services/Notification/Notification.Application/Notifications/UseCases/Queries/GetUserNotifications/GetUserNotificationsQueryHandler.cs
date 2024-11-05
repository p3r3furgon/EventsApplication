using AutoMapper;
using CommonFiles.Pagination;
using MediatR;
using Notifications.Application.Dtos;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, GetUserNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IMapper _mapper;

        public GetUserNotificationsQueryHandler(INotificationsRepository notificationsRepository, IMapper mapper)
        {
            _notificationsRepository = notificationsRepository;
            _mapper = mapper;
        }

        public async Task<GetUserNotificationsResponse> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userNotifications = await _notificationsRepository
                .GetByUserId(request.UserId, request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var userNotificationsDto = _mapper.Map<List<NotificationResponseDto>>(userNotifications);

            var pagedResponse = new PagedResponse<NotificationResponseDto>(userNotificationsDto,
                request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            return new GetUserNotificationsResponse(pagedResponse);
        }
    }
}
