using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        //Create a dependency to the Core Module
        private readonly IUnitOfWork _unitOfWork;


        public FollowingsController()
        {
            //Generate the access to the database and the Repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto) //Dto holds Artist Id
        {
            //Get the User ID of the current user
            var currentUserId = User.Identity.GetUserId();


            //Verify if there is record for the current user and the given artist
            if (_unitOfWork.IRepoFollowings.FollowingExist(dto, currentUserId)) //Check for the artist id
                return BadRequest("Following already exists");
            


            var following = new Following()
            {
                FollowerId = currentUserId,
                ArtistId = dto.ArtistId
            };


            //Create the Following
            _unitOfWork.IRepoFollowings.AddFollowing(following);
            _unitOfWork.Complete();

            return Ok();

        }



        [HttpDelete]
        public IHttpActionResult Unfollow(string id)
        {
            //Get the user ID
            var userId = User.Identity.GetUserId();

            //Get the following under the user ID and the artist ID
            var following = _unitOfWork.IRepoFollowings.GetFollowing(id, userId);

            
            if (following == null)
                return NotFound();

            _unitOfWork.IRepoFollowings.DeleteFollowing(following);
            _unitOfWork.Complete();

            return Ok(id);
        }

    }
}
