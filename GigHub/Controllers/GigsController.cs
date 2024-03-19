using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        //Create access to the Database
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        [Authorize]
        public ActionResult Create()
        {
            
            //Create the model
            var model = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            //Send the model to the view
            return View(model);
        }

        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGig(GigFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genres.ToList();
                
                return View("Create", model);

            }


            //Save the Gig in the database
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = model.GetDateTime(),
                GenreId = model.GenreId,
                Venue = model.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// </summary>
        /// <returns>A view with a list of the gigs the user will be attending. </returns>
        [Authorize]
        public ActionResult Attending()
        {
            //Get the id of the current user
            var curUserId = User.Identity.GetUserId();

            
            //Get the list of all the Gigs the current user will attend 
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == curUserId)//Filter by the current User Id
                .Select(a => a.Gig) //Select the gigs. At this point we get a collection of Gigs
                .Include(g=> g.Artist) //Eager load the Artist of each Gig
                .Include(g => g.Genre)//Eager load the Genre of each Gig
                .ToList();

            //Create the model
            var model = new GigsViewModel
            {
                UpcomingGigs = gigs,
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs"
            };

            //Send the model to the view
            return View("Gigs",model);

        }
    }
}