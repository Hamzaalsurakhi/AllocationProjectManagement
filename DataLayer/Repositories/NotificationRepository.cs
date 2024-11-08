using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly AppDbContext _appDbContext;
        public NotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Notification>> GetAllNotDeletedAsync()
        {
            return await _appDbContext.notifications.Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<List<Notification>> GetTopNotificationsAsync(int count)
        {
            return await _appDbContext.notifications
                                 .OrderByDescending(n => n.CreatedDate)
                                 .Take(count)
                                 .Where(x => !x.IsRead && !x.IsDeleted)
                                 .ToListAsync();
        }

    }
}
