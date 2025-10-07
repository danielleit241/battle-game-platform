namespace BattleGame.Shared
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

        public static IHostApplicationBuilder AddMessageBus(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessagePublisher>(sp =>
                new RabbitMqPublisher(
                    connectionString: builder.Configuration.GetConnectionString(Const.RabbitMq)
                            ?? throw new InvalidOperationException("RabbitMq connection string not configured"),
                    logger: sp.GetRequiredService<ILogger<RabbitMqPublisher>>()
                    )
            );

            builder.Services.AddSingleton<IMessageSubscriber>(sp =>
                new RabbitMqSubscriber(
                        connectionString: builder.Configuration.GetConnectionString(Const.RabbitMq)
                            ?? throw new InvalidOperationException("RabbitMq connection string not configured"),
                        logger: sp.GetRequiredService<ILogger<RabbitMqSubscriber>>()
                    )
            );

            return builder;
        }
    }
}
