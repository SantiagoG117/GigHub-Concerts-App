using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        //Create access to the database
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        

        public ActionResult Index(string query = null)
        {
            var curUserId = User.Identity.GetUserId();
  
            //If there is no query retrieve all upcoming gigs from the database
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled); // Get gigs that only exist in the future that haven't been canceled
                
                

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

            //Build the model
            var model = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs.ToList(),
                IsAuthenticatedUser = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
               
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