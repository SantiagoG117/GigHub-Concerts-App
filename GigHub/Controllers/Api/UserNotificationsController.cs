using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize] //API available to login user only
    public class UserNotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UserNotificationsController()
        {
            _context = new ApplicationDbContext();
        }
        public int GetNumberOfUnreadUserNotifications()
        {
            var curUserId = User.Identity.GetUserId();

            var unreadUserNotifications = _context.UserNotifications
                .Where(un => un.UserId == curUserId && !un.IsRead)
                .ToList();

            return unreadUserNotifications.Count();
        }
    }
}
