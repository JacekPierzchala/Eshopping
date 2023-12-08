namespace EventBus.Messages.Events;
public class BaseIntegrationEvent
{
    public Guid Id { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime CreatedDate { get; }

    public BaseIntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.Now;
    }
    public BaseIntegrationEvent(Guid id, DateTime createdDate)
    {
        Id = id;
        CreatedDate = createdDate;
    }
}
