﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize] //Since we need the id of the current user we must make sure this API is only accessible by authenticated users
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        /// <summary>
        /// Responds to api/Attendances/attend
        /// </summary>
        /// <param name="gigId">
        ///     ASP.NET API doesn't check for scalar parameter like integers in
        ///     the request body. It expects them to be in the URL. FromBody parameter
        ///     allows that.
        /// </param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto attendanceDto) //AttendanceDto dto
        {
            //Returns the ID of the current user
            var userId = User.Identity.GetUserId(); 

            //Check if there is an attendance for the current user for the given gig
            if (_context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == attendanceDto.GigId))
                return BadRequest("The attendance already exists.");

            //Create the attendance
            var attendance = new Attendance
            {
                GigId = attendanceDto.GigId,
                AttendeeId = userId 
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}