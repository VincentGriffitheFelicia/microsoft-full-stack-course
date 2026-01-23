using System;

public class AttendanceModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.Now;
    public bool Attended { get; set; } = false;
}