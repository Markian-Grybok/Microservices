using ContentService.Domain.Entities;
using MongoDB.Driver;

namespace ContentService.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<Content> Contents
        => _database.GetCollection<Content>("contents");
}
