﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    /// <summary>
    /// Association class between Notification and ApplicationUser
    /// </summary>
    public class UserNotification
    {
        //Identification properties
        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; }

        //Not-Nullable properties
        public bool IsRead { get; private set; }

        //Navigation properties (Shouldn't change after they are created, hence the private setters)
        public ApplicationUser User { get; private set; }
        public Notification Notification { get; private set; }

        

        /*
         * Custom constructor: Ensures that a UserNotification object is never created with Null values for the User and
         * Notification properties
         */
        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }
            
            if (notification == null)
            {
                throw new ArgumentNullException("Notification");
            }

            User = user;
            Notification = notification;
        }

        //Default constructor (Required for Entity Framework):
        protected UserNotification() {}


        /*
         * Behavior reach domain model. The controller's responsibility is to just
         * delegate actions. All behavior related to a domain should be encapsulated in the Model
         */
        public void Read()
        {
            IsRead = true;
        }
    }
}