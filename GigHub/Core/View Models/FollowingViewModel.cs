using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.View_Models
{
    public class FollowingViewModel
    {
        public IList<ApplicationUser> Artists { get; set; }
        public bool IsAuthenticatedUser { get; set; }
    }
}