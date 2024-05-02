using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;

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

                //Get the Gig we wish to update and the users attending to that Gig
                var gigInDb = _context.Gigs.
                    Include(g => g.Attendances.Select(a => a.Attendee)). //Eager load the attendees for the given Gig
                    Single(g => g.Id == model.Id && g.ArtistId == curUserId);

                //Update the Gig and notify the attendees
                gigInDb.Update(model);

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

            //Get the future attendances for the current user
            var attendances = _context.Attendances.
                Where(a => a.AttendeeId == curUserId && a.Gig.DateTime > DateTime.Now).
                ToList().
                /*
                       A lookup is a data structure that allows us to quickly lookup attendance by a given key (in
                       this case by gigId) because as we are rendering each gig we need to quickly look up if we have
                       an attendance or not.
                       */
                ToLookup(a => a.GigId);

            //Create the model
            var model = new GigsViewModel
            {
                UpcomingGigs = gigs,
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm attending",
                Attendances = attendances
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns> Sends a query string to the Index action in the Home controller </returns>
        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", 
                        "Home", 
                        //Query string to be sent to the Index action:
                        new { query = viewModel.SearchTerm});
        }


        public ActionResult Details(int gigId)
        {
            //Get the current user
            var curUserId = User.Identity.GetUserId();

            //Get the Gig and its Artist
            var gig = _context.Gigs
                    //Eager load the artist for the gig
                    .Include(g => g.Artist)
                    .SingleOrDefault(g => g.Id == gigId);

            if (gig == null)
                return HttpNotFound();

            var artist = gig.Artist;

            //Build the ViewModel
            var viewModel = new GigDetailsViewModel()
            {
                Gig = gig,
                ArtistName = artist.Name,
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                IsFollowingTheArtist = _context.Followings.Any(f => 
                                                               f.FollowerId == curUserId &&
                                                               f.ArtistId == artist.Id),
                IsAttending = _context.Attendances.Any(a => 
                                                        a.AttendeeId == curUserId &&
                                                        a.GigId == gig.Id)
            };

            return View("GigDetails", viewModel);
        }
    }
}