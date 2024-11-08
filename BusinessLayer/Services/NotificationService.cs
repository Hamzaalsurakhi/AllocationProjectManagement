using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.SignalR;
using DataLayer.Entities;
using Microsoft.AspNetCore.SignalR;


namespace BusinessLayer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        public async Task CreateNotificationAsync(string title, string message)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                IsRead = false,
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<NotificationDto>> GetTopNotificationsAsync(int count)
        {
            var Notifications = await _unitOfWork.NotificationRepository.GetTopNotificationsAsync(count);
            var NotificationDto = _mapper.Map<List<NotificationDto>>(Notifications);
            return NotificationDto;
        }



        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var Notification = await _unitOfWork.NotificationRepository.GetAllAsync();
            var NotificationDto = _mapper.Map<IEnumerable<NotificationDto>>(Notification);
            return NotificationDto;
        }
        public async Task DeleteAllNotificationsAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            _unitOfWork.NotificationRepository.RemoveRange(notifications);
            await _unitOfWork.CompleteAsync();
        }

        public async Task MarkAsReadByIdAsync(int notificationId, bool isRead)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {

                notification.IsRead = isRead;
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task MarkAllAsReadAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _unitOfWork.NotificationRepository.Update(notification);

            }
            await _unitOfWork.CompleteAsync();
        }
        public async Task MarkAllAsNotReadAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = false;
                _unitOfWork.NotificationRepository.Update(notification);

            }
            await _unitOfWork.CompleteAsync();
        }
        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                _unitOfWork.NotificationRepository.Remove(notification);
                await _unitOfWork.CompleteAsync();
            }
        }
        public async Task<bool> SoftDeleteNotification(int notificationId)
        {
            var notification = await _unitOfWork.GenericRepository<Notification>().GetByIdAsync(notificationId);

            if (notification == null)
            {
                return false;
            }

            notification.IsDeleted = true;

            _unitOfWork.GenericRepository<Notification>().Update(notification);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationNotDeleted()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllNotDeletedAsync();
            var NotificationDto = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return NotificationDto;
        }
    }
}
