﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.View_Models;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        //Create access to the database
        private readonly ApplicationDbContext _dbContext;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }

        

        public ActionResult Index()
        {
            //Retrieve all upcoming gigs from the database
            var upcomingGigs = _dbContext.Gigs
                .Include(g => g.Artist)
                .Where(g => g.DateTime > DateTime.Now) // Get gigs that only exist in the future
                .Include(g => g.Genre)
                .ToList(); 
            
            return View(upcomingGigs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}