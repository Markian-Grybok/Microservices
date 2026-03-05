using ContentService.Application.DTOs.Payloads;
using FluentResults;
using MediatR;

namespace ContentService.Application.Commands.CreateContent;

public record CreateContentCommand(
    string Name,
    Guid CreatedBy,
    PayloadDto Payload) : IRequest<Result<Guid>>;
