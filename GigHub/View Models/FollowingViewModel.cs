using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.View_Models
{
    public class FollowingViewModel
    {
        public IList<ApplicationUser> Artists { get; set; }
        public bool IsAuthenticatedUser { get; set; }
    }
}