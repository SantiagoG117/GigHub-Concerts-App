using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.View_Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;

namespace GigHub.Controllers
{
    public class GigController : Controller
    {
        //Create dependency to the Core module
        private readonly IUnitOfWork _unitOfWork;

        
        public GigController()
        {
            //Grants access to the database and Repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        protected override void Dispose(bool disposing)
        {
            new ApplicationDbContext().Dispose();
        }

        //Public Actions
        [Authorize]
        public ActionResult CreateGig()
        {
            
            //Create the model
            var model = new GigFormViewModel
            {
                Genres = _unitOfWork.IRepoGenres.GetGenres()
            };

            //Send the model to the view
            return View("GigsForm", model);
        }

        public ActionResult EditGig(int id)
        {

            //Get the gig from the database
            var gig = _unitOfWork.IRepoGigs.GetGig(id); 

            if (gig == null)
                return HttpNotFound();

            //Make sure the gig belongs to the logged in artist
            if (gig.ArtistId != GetLoggedInUserId())
                return new HttpUnauthorizedResult();

            //Build the model
            var model = new GigFormViewModel()
            {
                Id = gig.Id,
                Venue = gig.Venue,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                GenreId = gig.GenreId,
                Genres = _unitOfWork.IRepoGenres.GetGenres()
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
                model.Genres = _unitOfWork.IRepoGenres.GetGenres();
                
                return View("GigsForm", model);
            }

            

            //Update the gig in the database
            if (model.Id != 0)
            {
                //Get the Gig we wish to update and the users attending to that Gig
                var gigInDb = _unitOfWork.IRepoGigs.GetGigWithAttendees(model.Id);

                if (gigInDb == null)
                    return HttpNotFound();

                //Make sure the gig belongs to the logged in artist
                if (gigInDb.ArtistId != GetLoggedInUserId())
                    return new HttpUnauthorizedResult();

                //Update the Gig and notify the attendees
                gigInDb.Update(model);

            }
            //Create a new gig
            else
            {
                //Save the Gig in the database
                var gig = new Gig
                {
                    ArtistId = GetLoggedInUserId(),
                    DateTime = model.GetDateTime(),
                    GenreId = model.GenreId,
                    Venue = model.Venue
                };

                _unitOfWork.IRepoGigs.AddGig(gig);
            }

            

            //Save the changes
            _unitOfWork.Complete();


            return RedirectToAction("MyGigs", "Gig");
        }

        /// <summary>
        /// </summary>
        /// <returns>A view with a list of the gigs the user will be attending. </returns>
        [Authorize]
        public ActionResult Attending()
        {
            //Get the id of the current user
            var curUserId = GetLoggedInUserId();

            //Create the model
            var model = new GigsViewModel
            {
                UpcomingGigs = _unitOfWork.IRepoGigs.GetGigUserIsAttending(curUserId),
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm attending",
                /*
                     A lookup is a data structure that allows us to quickly lookup attendance by a given key (in
                     this case by gigId) because as we are rendering each gig we need to quickly look up if we have
                     an attendance or not.
                  */
                Attendances = _unitOfWork.IRepoAttendance.GetFutureAttendances(curUserId).ToLookup(a => a.GigId)
            };

            //Send the model to the view
            return View("Gigs",model);

        }

        public ActionResult MyGigs()
        {
         
            var curUserId = GetLoggedInUserId();

         
            var gigs = _unitOfWork.IRepoGigs.GetUpcomingGigsByArtist(curUserId);

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
            var curUserId = GetLoggedInUserId();

            //Get the Gig and its Artist
            var gig = _unitOfWork.IRepoGigs.GetGigWithArtist(gigId);

            if (gig == null)
                return HttpNotFound();

            //Get the followings of the current user
            var followings = _unitOfWork.IRepoFollowings
                                                    .GetArtistsFollowed(curUserId)
                                                    .ToLookup(f => f.ArtistId);


            //Build the ViewModel
            var viewModel = new GigDetailsViewModel()
            {
                Gig = gig,
                ArtistName = gig.Artist.Name,
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Followings = followings,
                IsAttending = _unitOfWork.IRepoAttendance.GetAttendance(curUserId, gigId) != null
            };

            return View("GigDetails", viewModel);
        }


        //Private method:
        private string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }

    }
}