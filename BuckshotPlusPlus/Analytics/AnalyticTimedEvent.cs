using System;

namespace BuckshotPlusPlus.Analytics;

public class AnalyticTimedEvent
{
    public string EventName { get; set; }
    public string EventTimestamp { get; set; }

    public AnalyticTimedEvent(string Event)
    {
        var Now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        EventTimestamp = Now.ToString();
        EventName = Event;

    }
}
