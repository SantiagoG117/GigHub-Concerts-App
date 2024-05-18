using GigHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Core.IRepositories;

namespace GigHub.Persistance.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Notification> GetNotificationsForUser(string curUserId)
        {
            return _context.UserNotifications.
                Where(un => un.UserId == curUserId).//Get the unread user notifications for the current user
                Select(un => un.Notification). //Select the Notification objects
                Include(n => n.Gig.Artist). //Eager load the Gig and Artist of each notification
                ToList();
        }

        public List<UserNotification> GetUnreadUserNotifications(string curUserId)
        {
            return _context.UserNotifications
                .Where(un => un.UserId == curUserId && !un.IsRead)
                .ToList();
        }


    }
}