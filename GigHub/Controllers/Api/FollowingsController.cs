using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Dtos;
using GigHub.Models;
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
        public IHttpActionResult Unfollow(string Id)
        {
            //Get the User ID of the current user
            var currentUserId = User.Identity.GetUserId();

            //Verify if there is a record for the current user and the given artist
            if (!_context.Followings.Any(f =>
                    f.FollowerId == currentUserId && //Check for the user id
                    f.ArtistId == Id)) //Check for the artist id
                return BadRequest("The user is not following this artist");

            var followingToBeRemoved = new Following()
            {
                FollowerId = currentUserId,
                ArtistId = Id
            };

            _context.Followings.Remove(followingToBeRemoved);
            _context.SaveChanges();

            return Ok();
        }
    }
}
