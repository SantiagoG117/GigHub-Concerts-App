using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GigHub.Core;
using GigHub.Core.IRepositories;
using GigHub.Core.Models;
using GigHub.Persistance.Repositories;

namespace GigHub.Persistance
{

    /*
     * Unit of Work stores the logic for storing or updating the database (writing changes) and provides
     * access to the Repositories.
     *
     * All Repositories should be only be initialized within the constructor of the Unit of Work
     */

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        //Interface properties
        public IGigRepository IRepoGigs { get; private set; }
        public IAttendanceRepository IRepoAttendance { get; private set; }
        public IGenresRepository IRepoGenres { get; private set; }

        public IFollowingsRepository IRepoFollowings { get; private set; }
        


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            //Initialize the Repositories
            IRepoGigs = new GigRepository(_context);
            IRepoAttendance = new AttendanceRepository(_context);
            IRepoGenres = new GenresRepository(_context);
            IRepoFollowings = new FollowingsRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}