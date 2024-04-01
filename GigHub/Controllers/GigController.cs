using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class GigController : Controller
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


            return RedirectToAction("MyGigs", "Gig");
        }

        public ActionResult DeleteGig(int id)
        {
            //Get the id of the current Artist
            var curArtistId = User.Identity.GetUserId();

            //Get the gig we wish to cancel and the users attending to that gig
            var gig = _context.Gigs.
                Include(g => g.Attendances.Select(a => a.Attendee)). //Eager load the attendees of the Gig
                Single(g => g.Id ==id && g.ArtistId == curArtistId);

            //If the gig is already cancelled return not Found
            if (gig.IsCanceled)
                return HttpNotFound();

            //Cancel the gig and notify the attendees 
            gig.Cancel();

            _context.SaveChanges();

            
            return RedirectToAction("MyGigs", "Gig");
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
                Heading = "Gigs I'm attending"
            };

            //Send the model to the view
            return View("Gigs",model);

        }

        public ActionResult MyGigs()
        {
            //Get the current user id
            var curUserId = User.Identity.GetUserId();

            //Get the upcoming gigs from the current user
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == curUserId //Gigs of the artist
                            && g.DateTime > DateTime.Now // that will happen in the future
                            && !g.IsCanceled) // and are not cancelled 
                .Include(g => g.Genre)// Eager load the Genres
                .ToList();

            //Send the gigs to the view
            return View(gigs);
        }
    }
}