using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize] //API available to login user only
    public class UserNotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNotificationsController()
        {
            //Grants access to the database and repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext()); ;
        }
        public int GetNumberOfUnreadUserNotifications()
        {
            var curUserId = User.Identity.GetUserId();

            var unreadUserNotifications = _unitOfWork.IRepoNotification
                                                            .GetUnreadUserNotifications(curUserId);
            /*
            var unreadUserNotifications = _context.UserNotifications
                .Where(un => un.UserId == curUserId && !un.IsRead)
                .ToList();
                */



            return unreadUserNotifications.Count();
        }
    }
}
