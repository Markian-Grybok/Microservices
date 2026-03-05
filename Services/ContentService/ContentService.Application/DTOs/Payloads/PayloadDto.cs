using System.Text.Json.Serialization;

namespace ContentService.Application.DTOs.Payloads;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TextPayloadDto), "text")]
[JsonDerivedType(typeof(MediaPayloadDto), "media")]
public abstract class PayloadDto {}