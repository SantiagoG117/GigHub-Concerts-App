using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GigHub.Core.IRepositories;
using GigHub.Core.Models;

namespace GigHub.Persistance.Repositories
{
    /*
     * Repositories mediate between the Business and the Data Access layer.They work like a List - or a Collection - of
     * Domain objects in memory: Add(object); Remove(object); Get(id). Its contract, or public methods, should not reflect
     * anything about a database (how to save or update data), this is the responsibility of the Unit of Work.
     *
     * Repositories are the only classes in our application responsible for data access, so they need to have a direct
     * reference to our Database System to access the data. This quality make repositories tightly coupled to the current
     * Database Framework of the application.  
     */

    public class GigRepository : IGigRepository
    {
        //Access to the database:
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void AddGig(Gig gig)
        {
            _context.Gigs.Add(gig);
        }

        public Gig GetGig(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == gigId);
        }
        
        public List<Gig> GetGigUserIsAttending(string curUserId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == curUserId)//Filter by the current User Id
                .Select(a => a.Gig) //Select the gigs. At this point we get a collection of Gigs
                .Include(g => g.Artist) //Eager load the Artist of each Gig
                .Include(g => g.Genre)//Eager load the Genre of each Gig
                .ToList();
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs.
                Include(g => g.Attendances.Select(a => a.Attendee)). //Eager load the attendees for the given Gig
                SingleOrDefault(g => g.Id == gigId);
        }

        public Gig GetGigWithArtist(int gigId)
        {
            return _context.Gigs
                        //Eager load the artist for the gig
                        .Include(g => g.Artist)
                        .SingleOrDefault(g => g.Id == gigId);
        }

        public IList<Gig> GetUpcomingGigsByArtist(string artistId)
        {
           return _context.Gigs
                        .Where(g => g.ArtistId == artistId
                                     && g.DateTime > DateTime.Now // that will happen in the future
                                     && !g.IsCanceled) // and are not cancelled 
                        .Include(g => g.Genre)// Eager load the Genres
                        .ToList();
        }

        public IQueryable<Gig> GetUpcomingGigs()
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);
        }


    }
}