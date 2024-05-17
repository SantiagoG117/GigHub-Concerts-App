using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
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

        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var curUserId = User.Identity.GetUserId();

            //Get the unread Notifications for the current user
            var notifications = _context.UserNotifications.
                Where(un => un.UserId == curUserId ).//Get the unread user notifications for the current user
                Select(un => un.Notification). //Select the Notification objects
                Include(n => n.Gig.Artist). //Eager load the Gig and Artist of each notification
                ToList();

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);

        }


        /// <summary>
        /// Mark the new notifications for the currently logged-in user as read.
        /// </summary>
        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            //Get the user ID
            var curUserId = User.Identity.GetUserId();

            //Get the unread User Notifications for the current user
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == curUserId && !un.IsRead)
                .ToList();

            /*
             * Set the notifications as read: Behavior reach domain model. The controller's responsibility is to just
             * delegate actions. All behavior related to a domain should be encapsulated in the Model
             */

            notifications.ForEach(n => n.Read());

            _context.SaveChanges();


            return Ok();
        }
    }
}
