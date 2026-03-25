namespace Ecosystem.Domain.Core.Events;

public abstract class Event
{
    public DateTime Timestamp { get; protected set; }
    public string CorrelationId { get; set; }

    protected Event()
    {
        Timestamp = DateTime.UtcNow;
        CorrelationId = Guid.NewGuid().ToString();
    }
}