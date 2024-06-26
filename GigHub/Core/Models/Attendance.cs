﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class Attendance
    {
        //Navigation properties
        public Gig Gig { get; set; }

        public ApplicationUser Attendee { get; set; }

        [Key] //Part of the composite Primary Key
        [Column(Order = 1)]
        public int GigId { get; set; } 


        [Key] //Part of the composite Primary Key
        [Column(Order = 2)]
        public string AttendeeId { get; set; } 
    }
}