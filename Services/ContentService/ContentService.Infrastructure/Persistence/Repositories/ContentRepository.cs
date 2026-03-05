using ContentService.Domain.Entities;
using ContentService.Domain.Interfaces;
using MongoDB.Driver;

namespace ContentService.Infrastructure.Persistence.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly MongoDbContext _dbContext;

    public ContentRepository(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Content> AddAsync(Content content, CancellationToken ct)
    {
        await _dbContext.Contents.InsertOneAsync(content, null, ct);

        return content;
    }

    public async Task<Content?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Content>.Filter.Eq(c => c.Id, id);
        var content = await _dbContext.Contents.Find(filter).FirstOrDefaultAsync(ct);

        return content;
    }
}
