using DataLayer.Entities;

namespace DataLayer.Interface
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetTopNotificationsAsync(int count);
        Task<List<Notification>> GetAllNotDeletedAsync();
    }
}
