namespace ContentService.Domain.Entities.Payloads;

public class TextPayload : ContentPayload
{
    public override string Type => "text";
    public string Text { get; set; } = string.Empty;
}