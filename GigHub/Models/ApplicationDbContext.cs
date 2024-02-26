using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //DbSets
        public DbSet<Gig> Gigs { get; set; } //Access to the Gig table
        public DbSet<Genre> Genres { get; set; } //Access to the Genres class
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}