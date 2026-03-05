namespace ContentService.Domain.Entities.Payloads;

public class MediaPayload : ContentPayload
{
    public override string Type => "media";
    public string Url { get; set; } = string.Empty;
}