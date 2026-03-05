using ContentService.Application.DTOs.Payloads;

namespace ContentService.Application.DTOs;

public record ContentDto(
    Guid Id,
    string Name,
    Guid CreatedBy,
    PayloadDto Payload);