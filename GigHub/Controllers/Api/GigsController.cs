using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {

            var artistId = User.Identity.GetUserId();

            //Get the gig we wish to cancel
            var gig = _context.Gigs.SingleOrDefault(g => 
                                                    g.Id == id //Get the gig with the passed ID
                                                    && g.ArtistId == artistId);// Make sure that only the Artist that created the ID can cancel it

            if (gig == null)
                return BadRequest("The requested gig does not exist");

            //Cancel the Gig and save the changes
            gig.IsCanceled = true;

            _context.SaveChanges();

            return Ok();
        }
    }
}
