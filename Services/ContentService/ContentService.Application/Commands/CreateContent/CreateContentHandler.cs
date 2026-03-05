using ContentService.Application.DTOs.Payloads;
using ContentService.Domain.Entities;
using ContentService.Domain.Entities.Payloads;
using ContentService.Domain.Interfaces;
using FluentResults;
using MediatR;

namespace ContentService.Application.Commands.CreateContent;

public class CreateContentHandler : IRequestHandler<CreateContentCommand, Result<Guid>>
{
    private readonly IContentRepository _contentRepository;

    public CreateContentHandler(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public async Task<Result<Guid>> Handle(CreateContentCommand request, CancellationToken ct)
    {
        try
        {
            ContentPayload? payload = request.Payload switch
            {
                TextPayloadDto dto => new TextPayload
                {
                    Text = dto.Text
                },
                MediaPayloadDto dto => new MediaPayload
                {
                    Url = dto.Url
                },
                _ => null
            };

            if (payload is null)
            {
                return Result.Fail(new Error("Unknown payload type.")
                    .WithMetadata("ErrorCode", "VALIDATION_ERROR"));
            }

            var content = Content.Create(
                request.Name,
                request.CreatedBy,
                payload);

            await _contentRepository.AddAsync(content, ct);

            return Result.Ok(content.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to create content.")
                .WithMetadata("ErrorCode", "DB_ERROR")
                .WithMetadata("Details", ex.InnerException?.Message ?? ex.Message));
        }
    }
}
