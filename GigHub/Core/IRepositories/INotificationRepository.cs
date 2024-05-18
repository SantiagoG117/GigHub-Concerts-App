using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface INotificationRepository
    {
        List<Notification> GetNotificationsForUser(string curUserId);
        List<UserNotification> GetUnreadUserNotifications(string curUserId);
    }
}