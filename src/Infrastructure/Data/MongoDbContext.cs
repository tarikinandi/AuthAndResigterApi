using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var databaseName = configuration["MongoDbSettings:DatabaseName"];

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "MongoDB bağlantı dizesi boş olamaz. Lütfen appsettings.json dosyanızı kontrol edin.");
        }

        if (string.IsNullOrEmpty(databaseName))
        {
            throw new ArgumentNullException(nameof(databaseName), "MongoDB veritabanı adı eksik. Lütfen appsettings.json dosyanızı kontrol edin.");
        }

        try
        {
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }
        catch (Exception ex)
        {
            throw new Exception($"MongoDB bağlantı hatası: {ex.Message}");
        }
    }
}
