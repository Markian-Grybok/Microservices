
using ContentService.Application.DTOs;
using ContentService.Application.DTOs.Payloads;
using ContentService.Domain.Entities.Payloads;
using ContentService.Domain.Interfaces;
using FluentResults;
using MediatR;

namespace ContentService.Application.Queries.GetContentById;

public class GetContentByIdHandler : IRequestHandler<GetContentByIdQuery, Result<ContentDto>>
{
    private readonly IContentRepository _contentRepository;

    public GetContentByIdHandler(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public async Task<Result<ContentDto>> Handle(GetContentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var content = await _contentRepository.GetByIdAsync(request.Id, ct);

            if (content is null)
            {
                return Result.Fail(new Error($"Content with id '{request.Id}' not found.")
                    .WithMetadata("ErrorCode", "NOT_FOUND"));
            }

            PayloadDto? payloadDto = content.Payload switch
            {
                TextPayload p => new TextPayloadDto
                {
                    Text = p.Text
                },
                MediaPayload p => new MediaPayloadDto
                {
                    Url = p.Url
                },
                _ => null
            };

            if (payloadDto is null)
            {
                return Result.Fail(new Error("Unknown payload type.")
                    .WithMetadata("ErrorCode", "VALIDATION_ERROR"));
            }

            return Result.Ok(new ContentDto(
                content.Id,
                content.Name,
                content.CreatedBy,
                payloadDto));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to get content.")
                .WithMetadata("ErrorCode", "DB_ERROR")
                .WithMetadata("Details", ex.InnerException?.Message ?? ex.Message));
        }
    }
}
