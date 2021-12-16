using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Denok.Lib.Database.Mongo.Ext 
{
    public static class MongoExt
    {
        public static bool Ping(this IMongoDatabase database, int secondToWait = 1)
        {
            if (secondToWait <= 0)
                throw new ArgumentOutOfRangeException("secondToWait", secondToWait, "Must be at least 1 second");
            return database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1*1000);
        }
    }
}