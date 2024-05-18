using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistance;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    [Authorize] //Since we need the id of the current user we must make sure this API is only accessible by authenticated users
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        public AttendancesController()
        {
            //Grants access to the database and Repositories
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }


        /// <summary>
        /// Responds to api/Attendances/
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto) 
        {
            //Returns the ID of the current user
            var userId = User.Identity.GetUserId(); 

            //Check if there is an attendance for the current user for the given gig
            if (_unitOfWork.IRepoAttendance.AttendanceExist(dto,userId))
                return BadRequest("The attendance already exists.");

            //Create the attendance
            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId 
            };

            _unitOfWork.IRepoAttendance.CreateAttendance(attendance);
            _unitOfWork.Complete();

            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult CancelAttend(int id)
        {
            //Get the current user ID
            var curUserId = User.Identity.GetUserId();

            //Get the attendance we wish to remove
            var attendance = _unitOfWork.IRepoAttendance.GetAttendance(curUserId, id);
            //Remove the attendance and save the changes
            if (attendance == null)
                return BadRequest("The attendance does not exist.");

            _unitOfWork.IRepoAttendance.DeleteAttendance(attendance);
            _unitOfWork.Complete();

            return Ok();
        }

    }
}
