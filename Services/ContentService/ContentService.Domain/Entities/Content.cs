using ContentService.Domain.Entities.Payloads;

namespace ContentService.Domain.Entities;

public class Content
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Guid CreatedBy { get; private set; }
    public ContentPayload Payload { get; private set; } = null!;

    private Content() { }

    public static Content Create(string name, Guid createdBy, ContentPayload payload)
    {
        return new Content
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedBy = createdBy,
            Payload = payload
        };
    }
}
