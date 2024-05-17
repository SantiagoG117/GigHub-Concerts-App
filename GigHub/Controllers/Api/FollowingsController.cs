using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto) //Dto holds Artist Id
        {
            //Get the User ID of the current user
            var currentUserId = User.Identity.GetUserId();


            //Verify if there is record for the current user and the given artist
            if (_context.Followings.Any(f => 
                                        f.FollowerId == currentUserId && //Check for the user id
                                        f.ArtistId == dto.ArtistId)) //Check for the artist id
                return BadRequest("Following already exists");
            


            var following = new Following()
            {
                FollowerId = currentUserId,
                ArtistId = dto.ArtistId
            };


            //Create the Following
            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();

        }

        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            //Get the user ID
            var userId = User.Identity.GetUserId();

            //Get the following under the user ID and the artist ID
            var following = _context.Followings
                .SingleOrDefault(f => f.FollowerId == userId && f.ArtistId == id);

            
            if (following == null)
                return NotFound();

            _context.Followings.Remove(following);
            _context.SaveChanges();

            return Ok(id);
        }
    }
}
