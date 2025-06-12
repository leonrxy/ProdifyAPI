using Microsoft.Extensions.Options;
using MongoDB.Driver;

public static class ServiceExtensions
{
    public static void AddMongo(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MongoSettings>(config.GetSection("MongoSettings"));
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });
        services.AddScoped(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });
    }
}