using ContentService.Domain.Interfaces;
using ContentService.Infrastructure.Mappings;
using ContentService.Infrastructure.Persistence;
using ContentService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ContentService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MongoMappings.Register();

        var connectionString = configuration["ConnectionStrings:Mongo"];
        var url = MongoUrl.Create(connectionString);
        var client = new MongoClient(url);
        var database = client.GetDatabase(url.DatabaseName);


        services.AddSingleton(database);
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IContentRepository, ContentRepository>();

        return services;
    }
}
