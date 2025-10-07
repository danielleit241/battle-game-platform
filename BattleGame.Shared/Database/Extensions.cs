namespace BattleGame.Shared.Database
{
    public static class Extensions
    {
        //public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration Configuration)
        //{
        //    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        //    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        //    services.AddSingleton<IMongoDatabase>(sp =>
        //    {
        //        var connectionString = Configuration.GetConnectionString(Const.GameDatabase) ?? throw new InvalidOperationException("Connection string 'MongoDb' not found.");
        //        var client = new MongoClient(connectionStrings.MongoDb);
        //        return client.GetDatabase(mongosettings.DatabaseName);
        //    });

        //    return services;
        //}
    }
}
