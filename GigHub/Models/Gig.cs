using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        //Navigation property to the ApplicationUser table
        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }// Foreign Key: ID's in  ApplicationUser class are string.

        
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        //Navigation property to the Genre table
        public Genre Genre { get; set; }

        [Required]
        public int GenreId { get; set; } //Foreign Key

    }
}