namespace BattleGame.Shared.Database
{
    public static class Extensions
    {
        //public static IHostApplicationBuilder AddMongoDb(this IHostApplicationBuilder builder, IConfiguration Configuration, string databaseName)
        //{
        //    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        //    builder.AddMongoDBClient("mongodb");
        //    builder.Services.AddSingleton<IMongoDatabase>(sp =>
        //    {
        //        var client = sp.GetRequiredService<IMongoClient>();
        //        var database = client.GetDatabase(databaseName);
        //        return database;
        //    });
        //}

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
