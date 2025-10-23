using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace BattleGame.Shared.Database
{
    public static class Extensions
    {
        public static IHostApplicationBuilder AddMongoDb(this IHostApplicationBuilder builder, string databaseName)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));

            builder.Services.AddSingleton<IMongoDatabase>(_ =>
            {
                var mongoConnectionString = builder.Configuration.GetConnectionString(databaseName);
                var client = new MongoClient(mongoConnectionString);
                return client.GetDatabase(databaseName);
            });

            return builder;
        }

        public static IHostApplicationBuilder AddNpgsqlDb<TContext>(
            this IHostApplicationBuilder builder,
            string databaseName)
            where TContext : DbContext
        {
            builder.AddNpgsqlDbContext<TContext>(databaseName, configureDbContextOptions: options =>
            {
                options.UseNpgsql(builder => builder.MigrationsAssembly(typeof(TContext).Assembly.FullName));
            });

            return builder;
        }
    }
}
