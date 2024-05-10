using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.View_Models
{
    
    public class GigDetailsViewModel
    {
        public Gig Gig { get; set; }
        public string ArtistName { get; set; }
        public bool IsAuthenticatedUser { get; set; }
        public bool IsAttending { get; set; }
        public ILookup<string, Following> Followings { get; set; }

        
    }
}