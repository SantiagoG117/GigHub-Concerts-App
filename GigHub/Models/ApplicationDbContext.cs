using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //DbSets
        public DbSet<Gig> Gigs { get; set; } //Access to the Gig table
        public DbSet<Genre> Genres { get; set; } //Access to the Genres class
        public DbSet<Attendance> Attendances { get; set; } // Access to the Attendances class

        public DbSet<Following> Followings { get; set; } // Access to the FollowArtists

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
            //Each attendance has a required Gig
            modelBuilder
                //Each attendance must have a Gig
                .Entity<Attendance>().HasRequired(a => a.Gig)
                //Each gig can have many attendances (People attending that gig)
                .WithMany()
                //Override Delete Cascade
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithRequired(f => f.Artist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Artists)
                .WithRequired(a => a.Follower)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}