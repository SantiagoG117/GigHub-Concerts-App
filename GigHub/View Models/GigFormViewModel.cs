using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.View_Models
{
    /// <summary>
    /// Model designed for presentation purposes of the Gig form.
    /// </summary>
    public class GigFormViewModel
    {
        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public int GenreId { get; set; } 


        public IList<Genre> Genres { get; set; }


        //Public methods
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
            

        }
    }
}