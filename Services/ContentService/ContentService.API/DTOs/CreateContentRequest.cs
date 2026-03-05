using ContentService.Application.DTOs.Payloads;

namespace ContentService.API.DTOs;

public record CreateContentRequest(
    string Name,
    PayloadDto Payload);