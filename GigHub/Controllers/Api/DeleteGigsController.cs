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
    public class DeleteGigsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public DeleteGigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var artistId = User.Identity.GetUserId();

            //Get the gig we wish to cancel
            var gig = _context.Gigs.SingleOrDefault(g => g.Id == id && g.ArtistId == artistId);

            gig.IsCanceled = true;

            _context.SaveChanges();

            return Ok();
        }
    }
}
