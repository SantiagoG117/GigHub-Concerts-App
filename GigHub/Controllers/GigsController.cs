using System;
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
        public ActionResult SaveGig(GigFormViewModel model)
        {

            //Save the Gig in the database
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = model.DateTime,
                GenreId = model.GenreId,
                Venue = model.Venue

            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}