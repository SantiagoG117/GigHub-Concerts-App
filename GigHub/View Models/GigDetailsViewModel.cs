using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.View_Models
{
    // ApplicationUser: Followers Whether the user is following the artist
    // Gig.Attendances: Whether the user is going to the Gig
    public class GigDetailsViewModel
    {
        public Gig Gig { get; set; }
        public string ArtistName { get; set; }
        public Attendance Attendance { get; set; }
        public bool IsAuthenticatedUser { get; set; }
        public bool IsFollowingTheArtist { get; set; }
        
    }
}