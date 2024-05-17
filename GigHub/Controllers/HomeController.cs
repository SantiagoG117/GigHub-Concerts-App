using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.View_Models;
using GigHub.Persistance;
using GigHub.Persistance.Repositories;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public HomeController()
        {
            // Generate access to the database and the Repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        
        public ActionResult Index(string query = null)
        {
            var curUserId = User.Identity.GetUserId();
  
            //If there is no query retrieve all upcoming gigs from the database
            var upcomingGigs = _unitOfWork.IRepoGigs.GetUpcomingGigs(); // Get gigs that only exist in the future that haven't been canceled
                
                

            //If there is a query string apply a filter to the search
            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs
                    // Lookup by three attributes, Artist Name, Genre or Location (Venue)
                    .Where(g =>
                        g.Artist.Name.Contains(query) ||
                        g.Genre.Name.Contains(query) ||
                        g.Venue.Contains(query));
            }


            //Build the model
            var model = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs.ToList(),
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                /*
                   A lookup is a data structure that allows us to quickly lookup attendance by a given key (in
                   this case by gigId) because as we are rendering each gig we need to quickly look up if we have
                   an attendance or not.
                   */
                Attendances = _unitOfWork.IRepoAttendance.GetFutureAttendances(curUserId).ToLookup(a => a.GigId)
               
            };
            
            return View("Gigs", model);
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}