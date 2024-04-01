using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //DbSets
        public DbSet<Gig> Gigs { get; set; } //Access to the Gig table
        
        public DbSet<Genre> Genres { get; set; } //Access to the Genres table
        
        public DbSet<Attendance> Attendances { get; set; } // Access toS the Attendances table
        
        public DbSet<Following> Followings { get; set; } // Access to the FollowArtists table

        public DbSet<Notification> Notifications { get; set; } // Access to the Notifications table

        public DbSet<UserNotification> UserNotifications { get; set; } //Access to the UserNotification association table

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //**************** Relationship between application users and the Gigs they are attending ****************

            //Each attendance has a required Gig
            modelBuilder
                //Each attendance must have a Gig
                .Entity<Attendance>().HasRequired(a => a.Gig)
                //Each gig can have many attendances (People attending that gig)
                .WithMany(g => g.Attendances)
                //Override Delete Cascade
                .WillCascadeOnDelete(false);

            //**************** Relationship between application user and its followers and artists ****************

            //An application user has many Followers (Collection)
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Followers)
                //Each follower has a required artist 
                .WithRequired(f => f.Artist)
                //Turn off cascade delete
                .WillCascadeOnDelete(false);

            //An application user has many Artist (Collection)
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Artists)
                //Each artist has a required follower
                .WithRequired(a => a.Follower)
                .WillCascadeOnDelete(false);

            //**************** Relationship between UserNotifications user and ApplicationUsers ****************

            //Each user notification has one and only one user
            modelBuilder.Entity<UserNotification>()
                .HasRequired(n => n.User)
                //Each user can have many user notifications
                .WithMany(u => u.UserNotifications)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}