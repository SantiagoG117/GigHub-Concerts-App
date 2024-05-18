using System.Collections.Generic;
using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IFollowingsRepository
    {
        Following GetFollowing(string artistId, string followerId);
        IList<Following> GetArtistsFollowed(string userId);
        bool FollowingExist(FollowingDto dto, string currentUserId);
        void AddFollowing(Following following);
        Following DeleteFollowing(Following following);
    }
}