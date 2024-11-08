
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BusinessLayer.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var notifications = await _notificationService.GetTopNotificationsAsync(5);
            await Clients.Caller.SendAsync("ReceiveNotification", notifications);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(IEnumerable<NotificationDto> notifications)
        {
            await Clients.All.SendAsync("ReceiveNotification", notifications);
        }
    }
}
