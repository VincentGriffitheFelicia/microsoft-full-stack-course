using System.Collections.Generic;
using System.Linq;

public static class AttendanceService
{
    private static List<AttendanceModel> _attendances = new List<AttendanceModel>();
    private static int _nextId = 1;

    public static IReadOnlyList<AttendanceModel> GetAll() => _attendances.AsReadOnly();
    public static AttendanceModel? GetById(int id) => _attendances.Find(a => a.Id == id);
    public static IReadOnlyList<AttendanceModel> GetByUserId(int userId) => _attendances.Where(a => a.UserId == userId).ToList().AsReadOnly();
    public static IReadOnlyList<AttendanceModel> GetByEventId(int eventId) => _attendances.Where(a => a.EventId == eventId).ToList().AsReadOnly();
    public static void Add(AttendanceModel attendance)
    {
        attendance.Id = _nextId++;
        _attendances.Add(attendance);
    }
    public static void Update(AttendanceModel attendance)
    {
        var idx = _attendances.FindIndex(a => a.Id == attendance.Id);
        if (idx >= 0)
        {
            _attendances[idx] = attendance;
        }
    }
}