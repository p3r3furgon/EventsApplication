using AutoMapper;
using CommonFiles.Pagination;
using MediatR;
using Notifications.Application.Dtos;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Application.Notifications.UseCases.Queries.GetAllNotifications
{
    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, GetAllNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IMapper _mapper;

        public GetAllNotificationsQueryHandler(INotificationsRepository notificationsRepository, IMapper mapper)
        {
            _notificationsRepository = notificationsRepository;
            _mapper = mapper;
        }

        public async Task<GetAllNotificationsResponse> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationsRepository
                .GetAll(request.PaginationParams.PageNumber, request.PaginationParams.PageSize);

            var notificationsDto = _mapper.Map<List<NotificationResponseDto>>(notifications);

            var pagedResponse = new PagedResponse<NotificationResponseDto>(notificationsDto, 
                request.PaginationParams.PageNumber, request.PaginationParams.PageSize);
            return new GetAllNotificationsResponse(pagedResponse);
        }
    }
}
