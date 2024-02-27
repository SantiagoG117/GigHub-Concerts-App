using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;

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


        // GET: Gigs
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

        public ActionResult SaveGig(GigFormViewModel model)
        {
            return View();
        }
    }
}