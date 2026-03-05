using ContentService.Application.DTOs;
using FluentResults;
using MediatR;

namespace ContentService.Application.Queries.GetContentById;

public record GetContentByIdQuery(Guid Id) : IRequest<Result<ContentDto>>;
