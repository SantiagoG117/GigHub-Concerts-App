﻿using System;
using GigHub.Core.Models;

namespace GigHub.Core.Dtos
{
    public class NotificationDto
    {
        
        public DateTime DateTime { get; set; }


        public DateTime? OriginalDaeDateTime { get; set; }
        public string OriginalVenue { get; set; }

        public NotificationType NotificationType { get; set; }
    
        public GigDto Gig { get; set; }
    }
}