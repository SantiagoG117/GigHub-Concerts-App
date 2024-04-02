using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize] //API available to login user only
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var curUserId = User.Identity.GetUserId();

            //Get the unread User Notifications for the current user
            var notifications = _context.UserNotifications.
                Where(un => un.UserId == curUserId && !un.IsRead).
                Select(un => un.Notification). //Select the Notification objects
                Include(n => n.Gig.Artist). //Eager load the Gig and Artist of each notification
                ToList();


            return notifications.Select(Mapper.Map<Notification, NotificationDto>);

        }
    }
}
