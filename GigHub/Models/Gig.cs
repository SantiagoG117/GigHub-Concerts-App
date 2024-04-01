using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace GigHub.Models
{
    public class Gig
    {
        //Identifications properties
        public int Id { get; private set; }


        public bool IsCanceled { get; private set; }


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

        public ICollection<Attendance> Attendances { get; private set; }

        //Constructor
        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        //Methods:
        public void Cancel()
        {
            //Set the cancel parameter to true
            IsCanceled = true;

            //Set the notification
            var notification = new Notification(NotificationType.GigCanceled, this);

            //Create a notification object for each attendee of the canceled Gig
            foreach (var attendee in Attendances.Select(a => a.Attendee))
                attendee.Notify(notification);
        }
    }
}