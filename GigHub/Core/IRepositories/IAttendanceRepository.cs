using System.Collections.Generic;
using GigHub.Core.Models;

namespace GigHub.Core.IRepositories
{
    public interface IAttendanceRepository
    {
        List<Attendance> GetFutureAttendances(string curUserId);
        Attendance GetAttendance(string userId, int gigId);
    }
}