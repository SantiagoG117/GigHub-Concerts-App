using System;
using System.Collections.Generic;
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

    public class AttendanceRepository : IAttendanceRepository
    {
        //Access to the database
        private readonly ApplicationDbContext _context;
        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Attendance> GetFutureAttendances(string curUserId)
        {
            return _context.Attendances.
                Where(a => a.AttendeeId == curUserId && a.Gig.DateTime > DateTime.Now).
                ToList();
        }

        public Attendance GetAttendance(string userId, int gigId)
        {
            return _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigId);
        }
        
    }
}