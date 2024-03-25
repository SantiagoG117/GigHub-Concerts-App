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
        public ActionResult CreateGig()
        {
            
            //Create the model
            var model = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            //Send the model to the view
            return View("GigsForm", model);
        }

        public ActionResult EditGig(int id)
        {
            //Get the id of the user
            var curUserId = User.Identity.GetUserId();

            //Get the gig from the database
            var gig = _context.Gigs.SingleOrDefault(g => g.Id == id && g.ArtistId == curUserId);

            if (gig == null)
            {
                return HttpNotFound();
            }

            //Build the model
            var model = new GigFormViewModel()
            {
                Id = gig.Id,
                Venue = gig.Venue,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                GenreId = gig.GenreId,
                Genres = _context.Genres.ToList()
            };


            //Send the model to the view

            return View("GigsForm", model);

        }

        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveGig(GigFormViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genres.ToList();
                
                return View("GigsForm", model);

            }

            

            //Update the gig in the database
            if (model.Id != 0)
            {
                //Get the id of the current user
                var curUserId = User.Identity.GetUserId();

                //Get the gig from id
                var gigInDb = _context.Gigs.Single(g => g.Id == model.Id && g.ArtistId == curUserId);

                //Update the gig to the values sent by the form
                gigInDb.Venue = model.Venue;
                gigInDb.DateTime = model.GetDateTime();
                gigInDb.GenreId = model.GenreId;

                
            }
            //Create a new gig
            else
            {
                //Save the Gig in the database
                var gig = new Gig
                {
                    ArtistId = User.Identity.GetUserId(),
                    DateTime = model.GetDateTime(),
                    GenreId = model.GenreId,
                    Venue = model.Venue
                };

                _context.Gigs.Add(gig);
            }

            

            //Save the changes
            _context.SaveChanges();

            return RedirectToAction("MyGigs", "Gigs");
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

        public ActionResult MyGigs()
        {
            //Get the current user if
            var curUserId = User.Identity.GetUserId();

            //Get the upcoming gigs from the current user
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == curUserId && g.DateTime > DateTime.Now)
                .Include(g => g.Genre)
                .ToList();

            //Send the gigs to the view
            return View(gigs);
        }
    }
}