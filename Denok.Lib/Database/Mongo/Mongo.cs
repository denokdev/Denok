using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Denok.Lib.Database.Mongo.Ext;

namespace Denok.Lib.Database.Mongo
{

    public interface IMongo
    {
        void Connect();
    
        IMongoDatabase Database();

        bool IsConnected();
    }

    public sealed class Mongo : IMongo, IDisposable
    {

        private readonly ILogger<Mongo> _logger;
        private IMongoClient _mongoClient;

        private MongoClientSettings _mongoClientSetting;

        private IMongoConfig _mongoConfig;

        public Mongo(ILogger<Mongo> logger, IMongoConfig mongoConfig)
        {
            _logger = logger;
            _mongoConfig = mongoConfig;
            _mongoClientSetting = 
                MongoClientSettings.FromConnectionString(mongoConfig.ConnectionString);
            
            
        }

        public void Connect()
        {
            _mongoClient = new MongoClient(_mongoClientSetting);
        }

        public IMongoDatabase Database()
        {
            if (_mongoClient == null)
                throw new NullReferenceException("_mongoClient is null");
            return _mongoClient.GetDatabase(_mongoConfig.DatabaseName);
        }

        public bool IsConnected() {
            if (Database() == null)
                throw new NullReferenceException("database is null");
            return Database().Ping();
        }

        public void Dispose()
        {
            
        }
    }
}