using ContentService.Domain.Entities;
using ContentService.Domain.Entities.Payloads;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace ContentService.Infrastructure.Mappings;

public static class MongoMappings
{
    public static void Register()
    {
        BsonSerializer.RegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));

        BsonSerializer.RegisterDiscriminatorConvention(
            typeof(ContentPayload),
            new ScalarDiscriminatorConvention("_t"));

        BsonClassMap.RegisterClassMap<ContentPayload>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
            cm.SetDiscriminatorIsRequired(true);
        });

        BsonClassMap.RegisterClassMap<TextPayload>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("text");
        });

        BsonClassMap.RegisterClassMap<MediaPayload>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("media");
        });

        BsonClassMap.RegisterClassMap<Content>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(c => c.Id);
        });

        var pack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCase", pack, _ => true);
    }
}