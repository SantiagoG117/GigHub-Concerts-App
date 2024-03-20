using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class FollowingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowingController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Following
        [Authorize]
        public ActionResult Following()
        {
            //Get the id of the current user
            var curUserId = User.Identity.GetUserId();

            //Get the artist that the current user is following
            var artists = _context.Followings
                .Where(f => f.FollowerId == curUserId)
                .Select(f => f.Artist)
                .ToList();

            //Build the model:
            var model = new FollowingViewModel
            {
                Artists = artists,
                IsAuthenticatedUser = User.Identity.IsAuthenticated
            };

            //Send the model the view:
            return View(model);
        }
    }
}