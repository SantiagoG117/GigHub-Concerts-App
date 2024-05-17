using System.Linq;
using GigHub.Core.Models;

namespace GigHub.Core.View_Models
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