using System;

namespace Denok.Lib.Database.Mongo
{

    public interface IMongoConfig
    {
        string ConnectionString { get; set;}

        string DatabaseName { get; set; }
    }

    public sealed class MongoConfig : IMongoConfig
    {
        public string ConnectionString { get; set ; }
        public string DatabaseName { get ; set ; }
    }
}