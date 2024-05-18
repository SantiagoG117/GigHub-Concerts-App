using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize] //API available to login user only
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController()
        {
            //Grants access to the database and repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var curUserId = User.Identity.GetUserId();

            //Get the unread Notifications for the current user
            var notifications = _unitOfWork.IRepoNotification.GetNotificationsForUser(curUserId);

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
            var notifications = _unitOfWork.IRepoNotification.GetUnreadUserNotifications(curUserId);

            /*
             * Set the notifications as read: Behavior reach domain model. The controller's responsibility is to just
             * delegate actions. All behavior related to a domain should be encapsulated in the Model
             */

            notifications.ForEach(n => n.Read());

            _unitOfWork.Complete();


            return Ok();
        }

    }
}
