using ContentService.Domain.Entities;

namespace ContentService.Domain.Interfaces;

public interface IContentRepository
{
    Task<Content?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Content> AddAsync(Content content, CancellationToken ct);
}