namespace Ecosystem.Domain.Core.Events;

public class Message
{
    public string MessageType { get; protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }
}