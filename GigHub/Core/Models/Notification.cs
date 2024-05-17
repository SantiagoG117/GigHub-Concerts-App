using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        //Identification properties
        public int Id { get; private set; } 

        //Not nullable properties
        public DateTime DateTime { get; private set; }

        //Nullable properties
        public DateTime? OriginalDaeDateTime { get; private set; }
        public string OriginalVenue { get; private set; }

        //Navigation properties
        public NotificationType NotificationType { get; private set; }
        [Required]
        public Gig Gig { get; set; }

        //Default constructor (Required for Entity Framework)
        public Notification()
        {
        }

        //Custom constructor:
        private Notification(NotificationType notificationType, Gig gig)
        {
            if (gig == null)
                throw new ArgumentNullException("Gig");

            DateTime = DateTime.Now;
            NotificationType = notificationType;
            Gig = gig;
        }

        /// <summary>
        /// Factory method: Responsible for creating an object in a valid state.
        /// 
        /// Responsible for creating a Notification object for a Gig that was just created
        /// </summary>
        /// <param name="gig"></param>
        /// <returns></returns>
        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }

        /// <summary>
        /// Factory method: Responsible for creating an object in a valid state.
        /// 
        /// Responsible for creating a Notification object for a Gig that was just updated
        /// </summary>
        /// <param name="newGig"></param>
        /// <param name="originalDateTime"></param>
        /// <param name="originalVenue"></param>
        /// <returns></returns>
        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            return new Notification(NotificationType.GigUpdated, newGig)
                    {
                        OriginalDaeDateTime = originalDateTime,
                        OriginalVenue = originalVenue
                    };

            
        }

        /// <summary>
        /// Factory method: Responsible for creating an object in a valid state.
        /// 
        /// Responsible for creating a Notification object for a Gig that was just cancelled
        /// </summary>
        /// <param name="gig"></param>
        /// <returns></returns>
        public static Notification GigCancelled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }

    }

}