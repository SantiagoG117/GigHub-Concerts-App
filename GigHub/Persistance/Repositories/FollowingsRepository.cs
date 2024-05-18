using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Dtos;
using GigHub.Core.IRepositories;
using GigHub.Core.Models;

namespace GigHub.Persistance.Repositories
{
    /*
     * Repositories mediate between the Business and the Data Access layer.They work like a List - or a Collection - of
     * Domain objects in memory: Add(object); Remove(object); Get(id). Its contract, or public methods, should not reflect
     * anything about a database (how to save or update data), this is the responsibility of the Unit of Work.
     *
     * Repositories are the only classes in our application responsible for data access, so they need to have a direct
     * reference to our Database System to access the data. This quality make repositories tightly coupled to the current
     * Database Framework of the application.
     */

    public class FollowingsRepository : IFollowingsRepository
    {
        //Create access to the database
        private readonly ApplicationDbContext _context;

        public FollowingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Following GetFollowing(string artistId, string followerId)
        {
            return _context.Followings
                .SingleOrDefault(f => f.FollowerId == followerId && f.ArtistId == artistId);
        }

        public IList<Following> GetArtistsFollowed(string userId)
        {
            return _context.Followings
                            .Where(f => f.FollowerId == userId)
                            .ToList();
        }

        public bool FollowingExist(FollowingDto dto, string currentUserId)
        {
            return _context.Followings.Any(f =>
                f.FollowerId == currentUserId && //Check for the user id
                f.ArtistId == dto.ArtistId);
        }

        public void AddFollowing(Following following)
        {
            _context.Followings.Add(following);
        }

        public Following DeleteFollowing(Following following)
        {
            return _context.Followings.Remove(following);
        }



    }
}