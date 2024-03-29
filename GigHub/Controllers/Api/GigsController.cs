using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Dtos;

namespace GigHub.Controllers
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var curUserId = User.Identity.GetUserId();
            //Get the Gig we wish to cancel
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == curUserId);

            //Set its canceled parameter to true
            gig.IsCanceled = true;
            _context.SaveChanges();

            return Ok();

        }
    }
}
