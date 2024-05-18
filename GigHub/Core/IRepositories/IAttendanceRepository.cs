using System.Collections.Generic;
using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IAttendanceRepository
    {
        List<Attendance> GetFutureAttendances(string curUserId);
        Attendance GetAttendance(string userId, int gigId);

        bool AttendanceExist(AttendanceDto dto, string userId);
        void CreateAttendance(Attendance attendance);
        void DeleteAttendance(Attendance attendance);

    }
}