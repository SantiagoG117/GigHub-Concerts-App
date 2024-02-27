using System;
using System.Collections.Generic;
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
        public string Venue { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        public int GenreId { get; set; } 
        public IList<Genre> Genres { get; set; }


    }
}