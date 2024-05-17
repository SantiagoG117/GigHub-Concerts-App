using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IFollowingsRepository
    {
        IList<Following> GetArtistsFollowed(string userId);
    }
}