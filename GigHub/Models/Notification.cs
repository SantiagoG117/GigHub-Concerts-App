using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GigHub.Controllers;

namespace GigHub.Models
{
    public class Notification
    {
        //Identification properties
        public int Id { get; private set; } 

        //Not nullable properties
        public DateTime DateTime { get; private set; }

        //Nullable properties
        public DateTime? OriginalDaeDateTime { get; set; }
        public string OriginalVenue { get; set; }

        //Navigation properties
        public NotificationType NotificationType { get; private set; }
        [Required]
        public Gig Gig { get; set; }

        //Default constructor (Required for Entoty Framework)
        public Notification()
        {
        }

        //Custom constructor:
        public Notification(NotificationType notificationType, Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("Gig");

            DateTime = DateTime.Now;
            NotificationType = notificationType;
            Gig = gig;
        }
    }
}