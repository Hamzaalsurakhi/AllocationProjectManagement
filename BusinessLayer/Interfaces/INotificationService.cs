using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string title, string message);

        Task<List<NotificationDto>> GetTopNotificationsAsync(int count);
        Task DeleteAllNotificationsAsync();
        Task MarkAsReadByIdAsync(int notificationId, bool isRead);
        Task DeleteNotificationAsync(int notificationId);
        Task MarkAllAsReadAsync();
        Task MarkAllAsNotReadAsync();
        Task<IEnumerable<NotificationDto>> GetAllNotificationNotDeleted();
        Task<bool> SoftDeleteNotification(int notificationId);
    }
}
