using CommonFiles.Interfaces;
using MediatR;
using Notifications.Domain.Interfaces;

namespace Notifications.Application.Notifications.UseCases.Commands.DeleteAllNotifications
{
    public class DeleteNotificationsCommandHandler : IRequestHandler<DeleteNotificationsCommand, DeleteNotificationsResponse>
    {
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteNotificationsCommandHandler(INotificationsRepository notificationsRepository, IUnitOfWork unitOfWork)
        {
            _notificationsRepository = notificationsRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteNotificationsResponse> Handle(DeleteNotificationsCommand request, CancellationToken cancellationToken)
        {

            _notificationsRepository.DeleteAll();
            await _unitOfWork.Save(cancellationToken);

            return new DeleteNotificationsResponse("Success");
        }
    }
}
