using System;
using System.Collections.Generic;

public static class EventService
{
    private static List<EventModel> _events = new List<EventModel>
    {
        new EventModel { Id = 1, Name = "Tech Conference 2026", Date = new DateTime(2026,3,15), Location = "Seattle, WA", Description = "A conference for modern web developers." },
        new EventModel { Id = 2, Name = "Art & Design Expo", Date = new DateTime(2026,4,10), Location = "Portland, OR", Description = "Showcase of design and creativity." },
        new EventModel { Id = 3, Name = "Startup Pitch Night", Date = new DateTime(2026,5,5), Location = "Online", Description = "Early-stage startups pitch to investors." }
    };

    public static IReadOnlyList<EventModel> GetAll() => _events.AsReadOnly();
    public static EventModel? GetById(int id) => _events.Find(e => e.Id == id);
    public static void Update(EventModel evt)
    {
        if (evt == null) throw new ArgumentNullException(nameof(evt));
        var idx = _events.FindIndex(x => x.Id == evt.Id);
        if (idx >= 0)
        {
            _events[idx] = evt;
        }
        else
        {
            // In a real app, throw or add new
            throw new InvalidOperationException("Event not found for update.");
        }
    }
}
