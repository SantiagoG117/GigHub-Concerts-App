using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using GigHub.Models;

namespace GigHub.View_Models
{
    public class HomeViewModel
    {
        public IList<Gig> UpcomingGigs { get; set; }

        public bool IsAuthenticatedUser { get; set; }
    }
}