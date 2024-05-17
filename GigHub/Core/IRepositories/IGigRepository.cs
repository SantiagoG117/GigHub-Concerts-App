using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IGigRepository
    {
        void AddGig(Gig gig);
        Gig GetGig(int gigId);
        List<Gig> GetGigUserIsAttending(string curUserId);
        Gig GetGigWithAttendees(int gigId);
        Gig GetGigWithArtist(int gigId);
        IList<Gig> GetUpcomingGigsByArtist(string artistId);
        IQueryable<Gig> GetUpcomingGigs();
    }
}