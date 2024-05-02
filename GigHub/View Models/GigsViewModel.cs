using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using GigHub.Models;

namespace GigHub.View_Models
{
    public class GigsViewModel
    {
        public IList<Gig> UpcomingGigs { get; set; }

        public bool IsAuthenticatedUser { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }

        public ILookup<int, Attendance> Attendances { get; set; }
    }
}